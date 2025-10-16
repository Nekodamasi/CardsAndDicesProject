using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public sealed class CombatStartSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<CombatStartSpawnInfo, GameObject> _CombatStartFactory;
        [Inject] private readonly CombatStartSpawnInfoManager _CombatStartSpawnInfoManager;
        [Inject] private readonly SceneInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _CombatStartSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var CombatStart = _CombatStartFactory(info);
                CombatStart.transform.SetParent(transform, false);
                CombatStart.transform.localScale = Vector3.one;
                foreach(var initializer in CombatStart.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

