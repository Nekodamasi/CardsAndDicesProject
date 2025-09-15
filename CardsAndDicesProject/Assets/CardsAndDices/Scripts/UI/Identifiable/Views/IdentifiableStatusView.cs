using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class IdentifiableStatusView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private Transform _moveTransform;
        [SerializeField] private GameObject _displayRootGameObject;
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] protected BoxCollider2D _boxCollider2D;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _hoverAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _normalAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _grayoutAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _dragAnimationStrategyEntity;

        private AnimationExecutor _animationExecutor = new AnimationExecutor();
        private Sequence _currentMoveAnimation;

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
        /// ホバー状態にします
        /// </summary>
        public Sequence DisplayHoverStatus()
        {

            return AnimationExecute(_hoverAnimationStrategyEntity);
        }

        /// <summary>
        /// ノーマル状態にします
        /// </summary>
        public Sequence DisplayNormalStatus()
        {

            return AnimationExecute(_normalAnimationStrategyEntity);
        }

        /// <summary>
        /// ハイド状態にします
        /// </summary>
        public void DisplayHideStatus()
        {
            SetDisplayActive(false);
        }

        /// <summary>
        /// グレイアウト状態にします
        /// </summary>
        public Sequence DisplayGrayoutStatus()
        {

            return AnimationExecute(_grayoutAnimationStrategyEntity);
        }

        /// <summary>
        /// ドラッグ状態にします
        /// </summary>
        public Sequence DisplayDragStatus()
        {

            return AnimationExecute(_dragAnimationStrategyEntity);
        }

        /// <summary>
        /// ドラッグ中の移動を行います
        /// </summary>
        public void MoveTo(Vector3 targetPosition)
        {
            Debug.Log("とらんすふぉーむぽじしょん：" + targetPosition);
            _moveTransform.position = targetPosition;
        }

        /// <summary>
        /// ドラッグ中の移動を行います
        /// </summary>
        public Sequence MoveToAnimated(Vector3 targetPosition, float animationDuration)
        {
            _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(_moveTransform.DOMove(targetPosition, animationDuration)
                                        .SetEase(Ease.OutQuad));
            return _currentMoveAnimation;
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

        /// <summary>
        /// ユーザーのインプットの有効／無効を切り替えます
        /// </summary>
        /// <param name="enable">trueで有効、falseで無効。</param>
        public void SetColliderEnabled(bool enable)
        {
            if (_boxCollider2D != null) _boxCollider2D.enabled = enable;
        }
    }
}
