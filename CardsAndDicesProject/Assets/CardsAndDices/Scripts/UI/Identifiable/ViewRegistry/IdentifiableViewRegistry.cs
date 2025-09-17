using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// シーン上の全てのBaseIdentifiableViewインスタンスを管理し、IDによる検索機能を提供するレジストリ。
    /// ScriptableObjectとして実装し、プロジェクト全体で単一のインスタンスを共有する。
    /// </summary>
    [CreateAssetMenu(fileName = "IdentifiableViewRegistry", menuName = "CardsAndDices/UI/Identifiable/ViewRegistry/IdentifiableViewRegistry")]
    public class IdentifiableViewRegistry : ScriptableObject
    {
        private readonly List<BaseIdentifiableView> _views = new();
        private readonly List<IdentifiableStatusView> _statusViews = new();
        private readonly List<DiceView> _diceViews = new();

        [Inject]
        public void Initialize()
        {
            _views.Clear();
            _statusViews.Clear();
        }

        /// <summary>
        /// Viewをレジストリに登録します。
        /// </summary>
        public void Register(BaseIdentifiableView view)
        {
            if (view == null || view.CompositeObjectId == null) return;

            if (view is IdentifiableStatusView statusView)
            {
                _statusViews.Add(statusView);
            }
        }

        /// <summary>
        /// Viewをレジストリから登録解除します。
        /// </summary>
        public void Unregister(BaseIdentifiableView view)
        {
        }

        /// <summary>
        /// 指定されたIDを持つViewを取得します。
        /// </summary>
        public T GetView<T>(CompositeObjectId id) where T : BaseIdentifiableView
        {
            return null;
        }

        /// <summary>
        /// 登録されている全てのIdentifiableStatusViewを取得します。
        /// </summary>
        public IReadOnlyList<IdentifiableStatusView> GetAllStatusViews() => _statusViews;

        /// <summary>
        /// 登録されている全てのDiceViewを取得します。
        /// </summary>
        public IReadOnlyList<DiceView> GetAllDiceViews() => _diceViews;
    }
}