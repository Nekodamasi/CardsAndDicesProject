using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// エネミークリーチャーカードPrefabからスポーンを行うクラス
    /// </summary>
    public sealed class EnemyCreatureCardSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<EnemyCreatureCardSpawnInfo, GameObject> _enemyCreatureCardFactory;
        [Inject] private readonly EnemyCreatureCardSpawnInfoManager _enemyCreatureCardSpawnInfoManager;
        [Inject] private readonly SceneInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _enemyCreatureCardSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var enemycreatureCard = _enemyCreatureCardFactory(info);
                enemycreatureCard.transform.SetParent(transform, false);
                enemycreatureCard.transform.localScale = Vector3.one;
                foreach(var initializer in enemycreatureCard.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

