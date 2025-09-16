using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public sealed class DiceSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<DiceSpawnInfo, GameObject> _diceFactory;
        [Inject] private readonly DiceSpawnInfoManager _diceSpawnInfoManager;
        [Inject] private readonly SceneInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _diceSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var dice = _diceFactory(info);
                dice.transform.SetParent(transform, false);
                dice.transform.localScale = Vector3.one;
                foreach(var initializer in dice.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

