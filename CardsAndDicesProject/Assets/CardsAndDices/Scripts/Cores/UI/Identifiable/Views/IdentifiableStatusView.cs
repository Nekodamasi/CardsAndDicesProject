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
        [SerializeField] protected SortingController _sortingController;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _hoverAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _normalAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _grayoutAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _dragAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _clickAnimationStrategyEntity;
        [SerializeField] private SEPlayer _sEPlayer;

        [Header("SEData Components")]
        [SerializeField] private SEData _hoverSeData;
        [SerializeField] private SEData _clickSeData;
        [SerializeField] private IdentifiableStatus _currentStatus = IdentifiableStatus.None;

        [Header("Sorting Components")]
        [SerializeField] private SortingOrderEntity _normalSortingOrderEntity;
        [SerializeField] private SortingOrderEntity _hoverSortingOrderEntity;
        [SerializeField] private SortingOrderEntity _dragSortingOrderEntity;
        

        private AnimationExecutor _animationExecutor = new AnimationExecutor();
        private Sequence _currentMoveAnimation;

        /// <summary>
        /// ソート順を変更します
        /// </summary>
        public void SetSortingOrder(SortingOrderEntity sortingOrderEntity)
        {
            if (sortingOrderEntity is null) return;
            _sortingController.SetOrder(sortingOrderEntity.OderValue);
        }

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
        /// 受け入れ状態にします
        /// </summary>
        public Sequence DisplayAcceptableStatus()
        {
            if (_currentStatus == IdentifiableStatus.Acceptable) return null;
            _currentStatus = IdentifiableStatus.Acceptable;
            SetColliderEnabled(true);
            SetDisplayActive(true);
            return null;
        }

        /// <summary>
        /// ホバー状態にします
        /// </summary>
        public Sequence DisplayHoverStatus()
        {
            if (_currentStatus == IdentifiableStatus.Hover) return null;
            _currentStatus = IdentifiableStatus.Hover;
            _sEPlayer.PlayOneShot(_hoverSeData);
            if (_hoverAnimationStrategyEntity is null)
            {
                return null;
            }

            SetSortingOrder(_hoverSortingOrderEntity);
            return AnimationExecute(_hoverAnimationStrategyEntity);
        }

        /// <summary>
        /// ノーマル状態にします
        /// </summary>
        public Sequence DisplayNormalStatus()
        {
            if (_currentStatus == IdentifiableStatus.Normal) return null;
            _currentStatus = IdentifiableStatus.Normal;
            SetColliderEnabled(true);
            SetDisplayActive(true);
            if (_normalAnimationStrategyEntity is null)
            {
                return null;
            }

            SetSortingOrder(_normalSortingOrderEntity);
            return AnimationExecute(_normalAnimationStrategyEntity);
        }

        /// <summary>
        /// ハイド状態にします
        /// </summary>
        public void DisplayHideStatus()
        {
            if (_currentStatus == IdentifiableStatus.Hide) return;
            _currentStatus = IdentifiableStatus.Hide;
            SetColliderEnabled(false);
            SetDisplayActive(false);
        }

        /// <summary>
        /// グレイアウト状態にします
        /// </summary>
        public Sequence DisplayGrayoutStatus()
        {
            if (_currentStatus == IdentifiableStatus.Grayout) return null;
            _currentStatus = IdentifiableStatus.Grayout;
            SetColliderEnabled(true);
            SetDisplayActive(true);

            SetSortingOrder(_normalSortingOrderEntity);
            return AnimationExecute(_grayoutAnimationStrategyEntity);
        }

        /// <summary>
        /// ドラッグ状態にします
        /// </summary>
        public Sequence DisplayDragStatus()
        {
            if (_currentStatus == IdentifiableStatus.DraggingStarted) return null;
            _currentStatus = IdentifiableStatus.DraggingStarted;
            SetColliderEnabled(false);
            SetDisplayActive(true);
            SetSortingOrder(_dragSortingOrderEntity);
            return AnimationExecute(_dragAnimationStrategyEntity);
        }

        /// <summary>
        /// インアクティブ状態にします
        /// </summary>
        public Sequence DisplayInactiveStatus()
        {
            if (_currentStatus == IdentifiableStatus.Inactive) return null;
            _currentStatus = IdentifiableStatus.Inactive;
            SetColliderEnabled(false);
            SetDisplayActive(true);
            SetSortingOrder(_normalSortingOrderEntity);
            return null;
        }

        /// <summary>
        /// クリック状態にします
        /// </summary>
        public Sequence DisplayClickStatus()
        {
            if (_currentStatus == IdentifiableStatus.Click) return null;
            _currentStatus = IdentifiableStatus.Click;
            SetColliderEnabled(true);
            SetDisplayActive(true);
            _sEPlayer.PlayOneShot(_clickSeData);
            SetSortingOrder(_normalSortingOrderEntity);
            return AnimationExecute(_clickAnimationStrategyEntity);
        }

        /// <summary>
        /// ドラッグ中の移動を行います
        /// </summary>
        public void MoveTo(Vector3 targetPosition)
        {
            _moveTransform.position = targetPosition;
        }

        /// <summary>
        /// 移動アニメーションを行います
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
