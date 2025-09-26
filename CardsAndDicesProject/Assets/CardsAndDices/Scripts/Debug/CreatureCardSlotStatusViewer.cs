using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotManagerが管理するインスタンスの状態をインスペクターに表示するためのデバッグ用クラス。
    /// </summary>
    public class CreatureCardSlotStatusViewer : MonoBehaviour
    {
        [System.Serializable]
        public class SlotStatus
        {
            public string name;
//            public Team team;
//            public LinePosition LinePosition;
//            public SlotLocation Location;
            public string PlacedCardId;
            public string ReflowPlacedCardId;
        }

        [Header("Dependencies")]
        [Inject] private readonly GameEventBus _gameEventBus;
        [Inject] private readonly CreatureCardSlotManager _creatureCardSlotManager;

        [Header("Status (Player Team Only)")]
        [SerializeField] private List<SlotStatus> _playerSlotStatuses = new List<SlotStatus>();

        private void Start()
        {
            // イベント購読
            _gameEventBus.On<IdentifiableStateBeginDragEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateClickEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateDragedHoverEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateDragEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateDropEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateEndDragEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateHoveredEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateHoverEvent>(OnStatusChanged);
            _gameEventBus.On<IdentifiableStateUnhoverEvent>(OnStatusChanged);
            _gameEventBus.On<ResetUIStatusEvent>(OnStatusChanged);

            // 初期表示
            UpdateDisplay();
        }

        private void OnDestroy()
        {
            // イベント購読解除
            if (_gameEventBus == null) return;
            _gameEventBus.Off<IdentifiableStateBeginDragEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateClickEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateDragedHoverEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateDragEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateDropEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateEndDragEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateHoveredEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateHoverEvent>(OnStatusChanged);
            _gameEventBus.Off<IdentifiableStateUnhoverEvent>(OnStatusChanged);
            _gameEventBus.Off<ResetUIStatusEvent>(OnStatusChanged);
        }

        private void OnStatusChanged(IEvent evt)
        {
            UpdateDisplay();
        }

        /// <summary>
        /// インスペクターの表示を更新します。
        /// </summary>
        private void UpdateDisplay()
        {
            if (_creatureCardSlotManager == null) return;

            _playerSlotStatuses.Clear();

            var instances = _creatureCardSlotManager.GetInstanceList();
            if (instances == null) return;

            var playerSlots = instances
                .Where(inst => inst.Team == Team.Player)
                .OrderBy(inst => inst.LinePosition)
                .ThenBy(inst => inst.Location);

            foreach (var instance in playerSlots)
            {
                string pid;
                if (instance.PlacedCardId is null)
                {
                    pid = null;
                }
                else
                {
                    pid = instance.PlacedCardId.ToString();
                }
                string rid;
                if (instance.ReflowPlacedCardId is null)
                {
                    rid = null;
                }
                else
                {
                    rid = instance.ReflowPlacedCardId.ToString();
                }

                _playerSlotStatuses.Add(new SlotStatus
                {
                    name = instance.Team.ToString() + "_" + instance.LinePosition.ToString() + "_" + instance.Location.ToString() + "[" + instance.CompositeObjectId.UniqueId + "]",
                    PlacedCardId = pid,
                    ReflowPlacedCardId = rid
                });
            }
        }
    }
}
