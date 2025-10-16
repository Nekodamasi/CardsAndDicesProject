using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public sealed class NextTurnSpawner : MonoBehaviour, IStartable
    {
        [Inject] private readonly Func<NextTurnSpawnInfo, GameObject> _NextTurnFactory;
        [Inject] private readonly NextTurnSpawnInfoManager _NextTurnSpawnInfoManager;
        [Inject] private readonly SceneInitializer _combatInitializer;

        public void Start()
        {
            var infolist = _NextTurnSpawnInfoManager.GetSpawnInfos();
            foreach (var info in infolist)
            {
                var NextTurn = _NextTurnFactory(info);
                NextTurn.transform.SetParent(transform, false);
                NextTurn.transform.localScale = Vector3.one;
                foreach(var initializer in NextTurn.GetComponentsInChildren<IGameInitializable>())
                {
                    _combatInitializer.AddInitializables(initializer);
                }
            }
        }
}
}

