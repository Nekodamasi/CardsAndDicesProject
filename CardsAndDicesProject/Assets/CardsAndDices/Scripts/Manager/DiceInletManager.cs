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
        private SpriteCommandBus _commandBus;
        private readonly Dictionary<CompositeObjectId, IDiceInlet> _inlets = new Dictionary<CompositeObjectId, IDiceInlet>();
        private readonly Dictionary<CompositeObjectId, DiceInletPresenter> _presenters = new();

        [Inject]
        public void Initialize(SpriteCommandBus commandBus)
        {
            ClearCollections();
            _commandBus = commandBus;
            _factory = new DiceInletFactory();
        }
        private void ClearCollections()
        {
            _inlets.Clear();
            foreach (var presenter in _presenters.Values)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }

        /// <summary>
        /// 新しいダイスインレットを生成し、管理下に置きます。
        /// </summary>
        public IDiceInlet CreateAndRegisterInlet(DiceInletView view, CompositeObjectId cardId, InletAbilityProfile profile)
        {
            var inletId = view.GetObjectId();
            var inlet = _factory.Create(inletId, cardId, profile);
            _inlets[inletId] = inlet;
            var presenter = new DiceInletPresenter(inlet, view, _commandBus);
            _presenters.Add(inletId, presenter);

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
        public void RemoveDiceInlet(CompositeObjectId id)
        {
            if (_presenters.TryGetValue(id, out var presenter))
            {
                presenter.Dispose();
                _presenters.Remove(id);
            }
            _inlets.Remove(id);
        }
    }
}
