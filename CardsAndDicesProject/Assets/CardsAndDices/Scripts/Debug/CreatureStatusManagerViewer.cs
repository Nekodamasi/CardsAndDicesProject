
using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// CreatureStatusManagerが管理するCreatureStatusInstanceの状態をインスペクターに表示するためのデバッグ用クラス。
    /// </summary>
    public class CreatureStatusManagerViewer : MonoBehaviour
    {
        [System.Serializable]
        public class CreatureStatus
        {
            public string InstanceId;
            public string Team;
            public int CurrentHealth;
            public int BaseHealth;
            public int CurrentShield;
            public int BaseShield;
            public int CurrentCooldown;
            public int BaseCooldown;
            public int Attack;
            public int Energy;
            public bool IsDeath;
            public bool IsDamage;
            public bool IsCooldownFinished;
        }

        [Header("Dependencies")]
        [Inject] private readonly GameEventBus _gameEventBus;
        [Inject] private readonly ICreatureStatusInstanceRepository _creatureStatusRepository;

        [Header("Status")]
        [SerializeField] private List<CreatureStatus> _creatureStatuses = new List<CreatureStatus>();

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
            _gameEventBus.On<UpdateDisplayCreatureStatusEvent>(OnStatusChanged);
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
            _gameEventBus.Off<UpdateDisplayCreatureStatusEvent>(OnStatusChanged);
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
            if (_creatureStatusRepository == null) return;

            _creatureStatuses.Clear();

            var instances = _creatureStatusRepository.GetInstanceList();
            if (instances == null) return;

            foreach (var instance in instances)
            {
                _creatureStatuses.Add(new CreatureStatus
                {
                    InstanceId = instance.CompositeObjectId?.ToString() ?? "null",
                    Team = instance.CreatureDataTeam.ToString(),
                    CurrentHealth = instance.CurrentHealth,
                    BaseHealth = instance.BaseHealth,
                    CurrentShield = instance.CurrentShield,
                    BaseShield = instance.BaseShield,
                    CurrentCooldown = instance.CurrentCooldown,
                    BaseCooldown = instance.BaseCooldown,
                    Attack = instance.Attack,
                    Energy = instance.Energy,
                    IsDeath = instance.IsDeath,
                    IsDamage = instance.IsDamage,
                    IsCooldownFinished = instance.IsCooldownFinished
                });
            }
        }
    }
}
