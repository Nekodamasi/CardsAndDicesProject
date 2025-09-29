using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace CardsAndDices
{
    public class SharedIconElementView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private SharedIconElementTypeEntity _sharedIconElementTypeEntity;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private GameObject _displayRootGameObject;
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _changeNumberAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _grayoutAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _normalAnimationStrategyEntity;
        [SerializeField] private string _formatString = "{0}";

        private AnimationExecutor _animationExecutor = new AnimationExecutor();

        /// <summary>
        /// Animationの実行を開始します。
        /// </summary>
        public Sequence AnimationExecute(AnimationStrategyEntity animationStrategyEntity)
        {
            var strategy = _animationStrategyRegistry.GetStrategy(animationStrategyEntity);
            var sequence = _animationExecutor.Execute(strategy, _animationContext);
            return sequence;
        }

        /// <summary>
        /// アイコンタイプ
        /// </summary>
        public SharedIconElementTypeEntity SharedIconElementTypeEntity => _sharedIconElementTypeEntity;

        public void UpdateNumberValue(int value)
        {
            _valueText.text = string.Format(_formatString, value);
        }

        /// <summary>
        /// ハイド状態にします
        /// </summary>
        public void DisplayHideStatus()
        {
            SetDisplayActive(false);
        }

         /// <summary>
        /// 番号が変更された際のアニメーションを実行します
        /// </summary>
        public Sequence DisplayChangeNumberAnimation(int value)
        {
            if (_changeNumberAnimationStrategyEntity is null)
            {
                UpdateNumberValue(value);
                return null;
            }
            UpdateNumberValue(value);
            return AnimationExecute(_grayoutAnimationStrategyEntity);
        }

        /// <summary>
        /// グレイアウト状態にします
        /// </summary>
        public Sequence DisplayGrayoutStatus()
        {
            SetDisplayActive(true);
            if (_grayoutAnimationStrategyEntity is null)
            {
                return null;
            }
            return AnimationExecute(_grayoutAnimationStrategyEntity);
        }

        /// <summary>
        /// ノーマル状態にします
        /// </summary>
        public Sequence DisplayNormalStatus()
        {
            SetDisplayActive(true);
            if (_normalAnimationStrategyEntity is null)
            {
                return null;
            }
            return AnimationExecute(_normalAnimationStrategyEntity);
        }

        /// <summary>
        /// 表示ルートGameObjectのアクティブ状態を設定します。
        /// </summary>
        /// <param name="active">trueで表示、falseで非表示。</param>
        public void SetDisplayActive(bool active)
        {
            if (_displayRootGameObject != null)
            {
                _displayRootGameObject.SetActive(active);
            }
        }
    }
}
