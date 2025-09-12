using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using UnityEditor.Rendering;

namespace CardsAndDices
{
    public class CombatLifetimeScope : LifetimeScope
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _creatureCardPrefab;

        [Header("ScriptableObject Managers")]
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private CreatureCardSpawnInfoManager _creatureCardSpawnInfoManager;
        [SerializeField] private IdentifiableCommandBus _identifiableCommandBus;
        [SerializeField] private CombatInitializer _combatInitializer;

        [Header("MonoBehaviour")]
        [SerializeField] private CreatureCardSpawner _creatureCardSpawner;

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインド
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableCommandBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatInitializer).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureCardSpawnInfoManager).AsSelf();

            // ScriptableObject Managers の初期化
            _compositeObjectIdManager.Initialize();
            _identifiableCommandBus.Initialize();
            _combatInitializer.Initialize(_identifiableCommandBus);

            // Factoryの登録
            builder.RegisterFactory<CreatureCardSpawnInfo, GameObject>(container => (info) =>
            {
                var card = container.Instantiate(_creatureCardPrefab);
                var IdentifiableGameObject = card.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = card.GetComponent<IdentifiableInputHandler>();
                            //                IdentifiableGameObject.Construct(_compositeObjectIdManager);
                            //                container.Inject(IdentifiableGameObject);
                return card;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_creatureCardSpawner);
        }
    }
}
