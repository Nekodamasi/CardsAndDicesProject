using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public sealed class CreatureCardSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<CreatureCardSpawnInfo, GameObject> _creatureCardFactory;
        [Inject] private readonly CreatureCardSpawnInfoManager _CreatureCardSpawnInfoManager;
        [Inject] private readonly CombatInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _CreatureCardSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var creatureCard = _creatureCardFactory(info);
                creatureCard.transform.SetParent(transform, false);
                creatureCard.transform.localScale = Vector3.one;
                foreach(var initializer in creatureCard.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

