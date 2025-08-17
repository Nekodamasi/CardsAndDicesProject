using System.Collections.Generic;
using VContainer;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ゲーム内に存在するすべてのDiceInletインスタンスを一元的に管理するクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletManager", menuName = "CardsAndDices/Managers/DiceInletManager")]
    public class DiceInletManager : ScriptableObject
    {
        private  DiceInletFactory _factory;
        private readonly Dictionary<CompositeObjectId, IDiceInlet> _inlets = new Dictionary<CompositeObjectId, IDiceInlet>();

        [Inject]
        public void Initialize()
        {
            _factory = new DiceInletFactory();
        }

        /// <summary>
        /// 新しいダイスインレットを生成し、管理下に置きます。
        /// </summary>
        public IDiceInlet CreateAndRegisterInlet(CompositeObjectId inletId, CompositeObjectId cardId, InletAbilityProfile profile)
        {
            var inlet = _factory.Create(inletId, cardId, profile);
            _inlets[inletId] = inlet;
            return inlet;
        }

        /// <summary>
        /// 指定されたIDのダイスインレットを取得します。
        /// </summary>
        /// <param name="id">取得するインレットのID</param>
        /// <returns>見つかったダイスインレット。見つからない場合はnull。</returns>
        public IDiceInlet GetInlet(CompositeObjectId id)
        {
            _inlets.TryGetValue(id, out var inlet);
            return inlet;
        }

        /// <summary>
        /// 指定されたIDのダイスインレットを管理下から削除します。
        /// </summary>
        /// <param name="id">削除するインレットのID</param>
        public void UnregisterInlet(CompositeObjectId id)
        {
            _inlets.Remove(id);
        }
    }
}
