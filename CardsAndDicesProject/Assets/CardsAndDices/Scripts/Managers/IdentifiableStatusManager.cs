using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全ての識別可能オブジェクトの状態(IdentifiableStatusInstance)を一元管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "IdentifiableStatusManager", menuName = "CardsAndDices/Managers/IdentifiableStatusManager")]
    public class IdentifiableStatusManager : ScriptableObject
    {
        private CompositeObjectRegistry _registry;
        private IdentifiableCommandBus _identifiableCommandBus;
        private IdentifiableUIStateMachine _identifiableUIStateMachine;
        private readonly Dictionary<CompositeObjectId, IdentifiableStatusInstance> _statusInstances = new();

        [Inject]
        public void Initialize(CompositeObjectRegistry registry, IdentifiableCommandBus identifiableCommandBus, IdentifiableUIStateMachine identifiableUIStateMachine)
        {
            _registry = registry;
            _identifiableCommandBus = identifiableCommandBus;
            _identifiableUIStateMachine = identifiableUIStateMachine;
            _identifiableCommandBus.On<SceneLoadedCommand>(OnSceneLoaded);
        }

        /// <summary>
        /// レジストリに登録された情報からインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedCommand cmd)
        {
            SetUpStatusInstances();
        }

        /// <summary>
        /// マネージャーを初期化し、Registryに登録されている全てのオブジェクトのステータスインスタンスを生成します。
        /// </summary>
        private void SetUpStatusInstances()
        {
            DisposeInstances();
            var allIds = _registry.GetAllCompositeObjectIds();
            foreach (var id in allIds)
            {
                if (id != null && !_statusInstances.ContainsKey(id))
                {
                    _statusInstances.Add(id, new IdentifiableStatusInstance(id, _identifiableCommandBus, _identifiableUIStateMachine));
                }
            }
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _statusInstances.Values)
            {
                instance.Dispose();
            }
            _statusInstances.Clear();
        }

        public void Dispose()
        {
            DisposeInstances();
        }

        /// <summary>
        /// 指定されたIDのインスタンスを返します。
        /// </summary>
        /// <param name="id">状態を取得したいオブジェクトのID。</param>
        /// <returns>現在の状態。対象が見つからない場合はNone。</returns>
        public IdentifiableStatusInstance GetStatus(CompositeObjectId id)
        {
            if (id != null && _statusInstances.TryGetValue(id, out var instance))
            {
                return instance;
            }
            return null;
        }
    }
}
