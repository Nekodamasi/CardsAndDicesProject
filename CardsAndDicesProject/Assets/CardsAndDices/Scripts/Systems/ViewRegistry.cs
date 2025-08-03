using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// シーン上の全てのBaseSpriteViewインスタンスを管理し、IDによる検索機能を提供するレジストリ。
    /// </summary>
    public class ViewRegistry
    {
        private readonly Dictionary<CompositeObjectId, BaseSpriteView> _views = new();
        private readonly List<CardSlotView> _slotViews = new();
        private readonly List<CreatureCardView> _creatureCardViews = new(); // CreatureCardViewのリストを追加

        /// <summary>
        /// Viewをレジストリに登録します。
        /// </summary>
        public void Register(BaseSpriteView view)
        {
            if (view == null || view.GetObjectId() == null) return;

            _views[view.GetObjectId()] = view;
            Debug.Log("ここがきーとうろく：" + view.GetObjectId());
            if (view is CardSlotView slotView)
            {
                _slotViews.Add(slotView);
            }
            else if (view is CreatureCardView creatureCardView) // CreatureCardViewの登録を追加
            {
                _creatureCardViews.Add(creatureCardView);
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
            else if (view is CreatureCardView creatureCardView) // CreatureCardViewの登録解除を追加
            {
                _creatureCardViews.Remove(creatureCardView);
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
        public IReadOnlyList<CreatureCardView> GetAllCreatureCardViews() => _creatureCardViews; // 新しいメソッドを追加

        public IReadOnlyDictionary<CompositeObjectId, BaseSpriteView> GetAllViews() => _views;
    }
}
