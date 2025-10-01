
using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// EffectManagerが管理するEffectInstanceの状態をインスペクターに表示するためのデバッグ用クラス。
    /// </summary>
    public class EffectManagerStatusViewer : MonoBehaviour
    {
        [System.Serializable]
        public class EffectStatus
        {
            public string TargetId;
            public string TargetType;
            public int Value;
            public int RemainingTurns;
            public string ExpiredTiming;
            public bool IsExpired;
        }

        [Header("Dependencies")]
        [Inject] private readonly GameEventBus _gameEventBus;
        [Inject] private readonly EffectManager _effectManager;

        [Header("Status")]
        [SerializeField] private List<EffectStatus> _effectStatuses = new List<EffectStatus>();

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
            if (_effectManager == null) return;

            _effectStatuses.Clear();

            var instances = _effectManager.GetInstanceList();
            if (instances == null) return;

            foreach (var instance in instances)
            {
                _effectStatuses.Add(new EffectStatus
                {
                    TargetId = instance.CompositeObjectId?.ToString() ?? "null",
                    TargetType = instance.TargetType.ToString(),
                    Value = instance.Value,
                    RemainingTurns = instance.RemainingTurns,
                    ExpiredTiming = instance.ExpiredTiming.ToString(),
                    IsExpired = instance.IsExpired
                });
            }
        }
    }
}
