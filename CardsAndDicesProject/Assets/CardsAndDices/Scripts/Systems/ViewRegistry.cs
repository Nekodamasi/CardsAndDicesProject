using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// シーン上の全てのBaseSpriteViewインスタンスを管理し、IDによる検索機能を提供するレジストリ。
    /// ScriptableObjectとして実装し、プロジェクト全体で単一のインスタンスを共有する。
    /// </summary>
    [CreateAssetMenu(fileName = "ViewRegistry", menuName = "CardsAndDices/Systems/ViewRegistry")]
    public class ViewRegistry : ScriptableObject
    {
        private readonly Dictionary<CompositeObjectId, BaseSpriteView> _views = new();
        private readonly List<CardSlotView> _slotViews = new();
        private readonly List<CreatureCardView> _creatureCardViews = new();
        private readonly List<DiceView> _diceViews = new();
        private readonly List<DiceSlotView> _diceSlotViews = new();
        private readonly List<DiceInletView> _inletViews = new();

        [Inject]
        public void Initialize()
        {
            _views.Clear();
            _slotViews.Clear();
            _creatureCardViews.Clear();
            _diceViews.Clear();
            _diceSlotViews.Clear();
            _inletViews.Clear();
        }

        /// <summary>
        /// Viewをレジストリに登録します。
        /// </summary>
        public void Register(BaseSpriteView view)
        {
            if (view == null || view.GetObjectId() == null) return;

            _views[view.GetObjectId()] = view;
            if (view is CardSlotView slotView)
            {
                _slotViews.Add(slotView);
            }
            else if (view is CreatureCardView creatureCardView)
            {
                _creatureCardViews.Add(creatureCardView);
            }
            else if (view is DiceView diceView)
            {
                _diceViews.Add(diceView);
            }
            else if (view is DiceSlotView diceSlotView)
            {
                _diceSlotViews.Add(diceSlotView);
            }
            else if (view is DiceInletView inletView)
            {
                _inletViews.Add(inletView);
            }
        }

        /// <summary>
        /// Viewをレジストリから登録解除します。
        /// </summary>
        public void Unregister(BaseSpriteView view)
        {
            if (view == null || view.GetObjectId() == null) return;

            _views.Remove(view.GetObjectId());
            if (view is CardSlotView slotView)
            {
                _slotViews.Remove(slotView);
            }
            else if (view is CreatureCardView creatureCardView)
            {
                _creatureCardViews.Remove(creatureCardView);
            }
            else if (view is DiceInletView inletView)
            {
                _inletViews.Remove(inletView);
            }
        }

        /// <summary>
        /// 指定されたIDを持つViewを取得します。
        /// </summary>
        public T GetView<T>(CompositeObjectId id) where T : BaseSpriteView
        {
            if (id != null && _views.TryGetValue(id, out var view))
            {
                return view as T;
            }
            return null;
        }

        /// <summary>
        /// 登録されている全てのCardSlotViewを取得します。
        /// </summary>
        public IReadOnlyList<CardSlotView> GetAllSlotViews() => _slotViews;

        /// <summary>
        /// 登録されている全てのCreatureCardViewを取得します。
        /// </summary>
        public IReadOnlyList<CreatureCardView> GetAllCreatureCardViews() => _creatureCardViews;
        public IReadOnlyList<DiceView> GetAllDiceViews() => _diceViews;
        public IReadOnlyList<DiceSlotView> GetAllDiceSlotViews() => _diceSlotViews;
        public IReadOnlyList<DiceInletView> GetAllInletViews() => _inletViews;

        public IReadOnlyDictionary<CompositeObjectId, BaseSpriteView> GetAllViews() => _views;
    }
}