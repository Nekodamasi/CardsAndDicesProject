using UnityEngine;
using UnityEngine.EventSystems;

namespace CardsAndDice
{
    /// <summary>
    /// SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。
    /// </summary>
    public class SpriteInputHandler : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler // OnDropを追加
    {
        [Header("Components")]
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private IdentifiableGameObject _identifiableGameObject;
        [SerializeField] private InteractionProfile _profile; // InteractionProfileへの参照を追加

        private bool _isHovering = false;
        private bool _isDragging = false;

        /// <summary>
        /// コンポーネントの初期化を行います。
        /// </summary>
        private void Awake()
        {
            // _commandBus はインスペクターで設定されるため、ここでは初期化不要
            // _identifiableGameObject もインスペクターで設定されるか、GetComponentで取得
            if (_identifiableGameObject == null)
            {
                _identifiableGameObject = GetComponent<IdentifiableGameObject>();
            }
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
            if (_profile != null && !_profile.CanHover) return; // ガード節を追加
            if (!_isDragging)
            {
                _isHovering = true;
                _commandBus.Emit(new SpriteHoverCommand(_identifiableGameObject.ObjectId));
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
                _commandBus.Emit(new SpriteUnhoverCommand(_identifiableGameObject.ObjectId));
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
                _commandBus.Emit(new SpriteClickCommand(_identifiableGameObject.ObjectId));
            }
            // OnEndDragでドロップ処理を行うため、ここでは特別な処理は不要
        }

        /// <summary>
        /// ドラッグ開始時の処理。
        /// </summary>
        /// <param name="eventData">ポインターイベントデータ</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_profile != null && !_profile.CanDrag) return; // ガード節を追加
            _isDragging = true;
            _commandBus.Emit(new SpriteBeginDragCommand(_identifiableGameObject.ObjectId));
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

                _commandBus.Emit(new SpriteDragCommand(_identifiableGameObject.ObjectId, newPosition));
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
                _commandBus.Emit(new SpriteEndDragCommand(_identifiableGameObject.ObjectId));
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

            _commandBus.Emit(new SpriteDropCommand(droppedObjectIdentifiable.ObjectId, targetObjectIdentifiable.ObjectId));
        }
    }
}
