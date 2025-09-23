using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public sealed class CreatureCardSlotSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<CreatureCardSlotSpawnInfo, GameObject> _creatureCardSlotFactory;
        [Inject] private readonly CreatureCardSlotSpawnInfoManager _CreatureCardSlotSpawnInfoManager;
        [Inject] private readonly SceneInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _CreatureCardSlotSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var creatureCardSlot = _creatureCardSlotFactory(info);
                creatureCardSlot.transform.SetParent(transform, false);
                creatureCardSlot.transform.localScale = Vector3.one;
                foreach(var initializer in creatureCardSlot.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

