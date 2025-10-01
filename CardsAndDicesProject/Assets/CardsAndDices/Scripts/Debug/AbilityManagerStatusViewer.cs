
using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// AbilityManagerが管理するAbilityInstanceの状態をインスペクターに表示するためのデバッグ用クラス。
    /// </summary>
    public class AbilityManagerStatusViewer : MonoBehaviour
    {
        [System.Serializable]
        public class AbilityStatus
        {
            public string OwnerId;
            public string SubOwnerId;
            public string AbilityName;
            public int RemainingUsages;
            public bool IsLock;
            public bool IsAvailable;
            public string ActivationTiming;
            public bool IsTrigger;
        }

        [Header("Dependencies")]
        [Inject] private readonly GameEventBus _gameEventBus;
        [Inject] private readonly AbilityManager _abilityManager;

        [Header("Status")]
        [SerializeField] private List<AbilityStatus> _abilityStatuses = new List<AbilityStatus>();

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
        }

        private void OnDestroy()
        {
            // イベント購読解除
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
            // Await one frame to ensure all event processing is complete
            // before updating the display.
            // This can help prevent issues where the data is read mid-update.
            // await UniTask.NextFrame();
            UpdateDisplay();
        }

        /// <summary>
        /// インスペクターの表示を更新します。
        /// </summary>
        private void UpdateDisplay()
        {
            if (_abilityManager == null) return;

            _abilityStatuses.Clear();

            var instances = _abilityManager.GetInstanceList();
            if (instances == null) return;

            foreach (var instance in instances)
            {
                _abilityStatuses.Add(new AbilityStatus
                {
                    OwnerId = instance.CompositeObjectId?.ToString() ?? "null",
                    SubOwnerId = instance.SubOwnerId?.ToString() ?? "null",
                    AbilityName = instance.BaseAbilityData != null ? instance.BaseAbilityData.name : "null SO",
                    RemainingUsages = instance.RemainingUsages,
                    IsLock = instance.IsLock,
                    IsAvailable = instance.IsAvailable,
                    ActivationTiming = instance.ActivationTiming.ToString(),
                    IsTrigger = instance.IsTrigger
                });
            }
        }
    }
}
