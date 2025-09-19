using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。
    /// </summary>
    public class IdentifiableInputHandler : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IGameInitializable
    {
        [Header("Components")]
        [SerializeField] private IdentifiableGameObject _identifiableGameObject;
        [SerializeField] private InteractionProfile _profile;
        [SerializeField] private BaseIdentifiableStateOperator _identifiableStateOperator;
        private GameEventBus _eventBus;
        private bool _isHovering = false;
        private bool _isDragging = false;

		[Inject]
		public void Construct(GameEventBus gameEventBus)
		{
			_eventBus = gameEventBus;
        }

		/// <summary>
		/// コンポーネントの初期化を行います。
		/// </summary>
		public void OnAwake()
		{
		}

        public void OnStart()
        {
            _identifiableStateOperator.RegisterTarget(_identifiableGameObject.CompositeObjectId);
		}

        /// <summary>
        /// マウスポインターがUI要素に入った時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void SetProfile(InteractionProfile newProfile)
        {
            _profile = newProfile;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("ほばーきてる？");
            if (_profile != null && !_profile.CanHover) return; // ガード節を追加
            if (!_isDragging)
            {
                _isHovering = true;
                _eventBus.Emit(new IdentifiableHoverEvent(_identifiableGameObject.CompositeObjectId));
            }
        }

        /// <summary>
        /// マウスポインターがUI要素から出た時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_profile != null && !_profile.CanHover) return; // ガード節を追加
            if (_isHovering && !_isDragging)
            {
                _isHovering = false;
                _eventBus.Emit(new IdentifiableUnhoverEvent(_identifiableGameObject.CompositeObjectId));
            }
        }

        /// <summary>
        /// マウスボタンが押された時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            // OnBeginDragでドラッグ開始処理を行うため、ここでは特別な処理は不要
        }

        /// <summary>
        /// マウスボタンが離された時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_profile != null && !_profile.CanClick) return; // ガード節を追加
            if (!_isDragging)
            {
                // ドラッグ中でなければクリックとみなす
                _eventBus.Emit(new IdentifiableClickEvent(_identifiableGameObject.CompositeObjectId));
            }
            // OnEndDragでドロップ処理を行うため、ここでは特別な処理は不要
        }

        /// <summary>
        /// ドラッグ開始時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("<color=red>OnBeginDrag元：</color>" + gameObject.name + "_" + _profile.name);
            if (_profile != null && !_profile.CanDrag) return; // ガード節を追加
            _isDragging = true;
            _eventBus.Emit(new IdentifiableBeginDragEvent(_identifiableGameObject.CompositeObjectId));
        }

        /// <summary>
        /// ドラッグ中の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (_profile != null && !_profile.CanDrag) return; // ガード節を追加
            if (_isDragging)
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
                newPosition.z = transform.position.z; // Z座標は変更しない

                _eventBus.Emit(new IdentifiableDragEvent(_identifiableGameObject.CompositeObjectId, newPosition));
            }
        }

        /// <summary>
        /// ドラッグ終了時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _eventBus.Emit(new IdentifiableEndDragEvent(_identifiableGameObject.CompositeObjectId));
            }
        }

        /// <summary>
        /// ドロップ時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnDrop(PointerEventData eventData)
        {
            if (_profile != null && !_profile.CanBeDropTarget) return; // ガード節を追加
            // SpriteInputHandlerのOnDropは、ドロップターゲットがない場所でのドロップも検知する
            // ドロップターゲットがある場合は、そのターゲットのOnDropが先に呼ばれる
            // ここでは、ドロップされたオブジェクトのObjectIdと、ドロップターゲットのObjectIdを渡す
            IdentifiableGameObject droppedObjectIdentifiable = eventData.pointerDrag.GetComponent<IdentifiableGameObject>();
            IdentifiableGameObject targetObjectIdentifiable = GetComponent<IdentifiableGameObject>(); // このオブジェクト自身がターゲット

            _eventBus.Emit(new IdentifiableDropEvent(droppedObjectIdentifiable.CompositeObjectId, targetObjectIdentifiable.CompositeObjectId));
        }
    }
}
