using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// VFXの再生とオブジェクトプールを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "VfxManager", menuName = "CardsAndDices/Managers/VfxManager")]
    public class VfxManager : ScriptableObject
    {
        private readonly Dictionary<VfxDefinition, Queue<VfxPlayer>> _pool = new Dictionary<VfxDefinition, Queue<VfxPlayer>>();
        private readonly Dictionary<int, VfxPlayer> _activeInstances = new Dictionary<int, VfxPlayer>();
        private int _nextInstanceId = 0;

        // DIコンテナから注入される依存性
        private Transform _poolParent;

        [Inject]
        public void Initialize()
        {
            // プーリングしたVFXの親となるオブジェクトをシーンに作成
            var poolParentGo = new GameObject("VfxPool");
            DontDestroyOnLoad(poolParentGo);
            _poolParent = poolParentGo.transform;

            _pool.Clear();
            _activeInstances.Clear();
            _nextInstanceId = 0;
        }

        /// <summary>
        /// 指定されたVFXを非同期で再生します。
        /// </summary>
        /// <param name="vfxDefinition">再生するVFXの定義。</param>
        /// <param name="position">再生する位置。</param>
        /// <param name="rotation">再生する回転。</param>
        /// <returns>再生インスタンスIDを含むUniTask。</returns>
        public async UniTask<int> PlayVfxAsync(VfxDefinition vfxDefinition, Vector3 position, Quaternion rotation)
        {
            VfxPlayer player = GetFromPool(vfxDefinition);
            if (player == null) return -1; // プレハブがないなどの理由でプレイヤーを取得できなかった

            player.transform.SetPositionAndRotation(position, rotation);
            player.gameObject.SetActive(true);

            int instanceId = _nextInstanceId++;
            _activeInstances.Add(instanceId, player);

            player.Play(vfxDefinition);

            await UniTask.Yield();
            return instanceId;
        }

        /// <summary>
        /// 指定されたインスタンスIDのVFXを停止します。
        /// </summary>
        /// <param name="instanceId">停止するVFXのインスタンスID。</param>
        public void StopVfx(int instanceId)
        {
            if (_activeInstances.TryGetValue(instanceId, out VfxPlayer player))
            {
                player.Stop();
            }
        }

        private VfxPlayer GetFromPool(VfxDefinition vfxDefinition)
        {
            if (!_pool.ContainsKey(vfxDefinition))
            {
                _pool[vfxDefinition] = new Queue<VfxPlayer>();
            }

            if (_pool[vfxDefinition].Count > 0)
            {
                VfxPlayer player = _pool[vfxDefinition].Dequeue();
                return player;
            }

            GameObject prefab = vfxDefinition.ParticlePrefab;
            if (prefab == null)
            {
                Debug.LogError($"VfxDefinition '{vfxDefinition.name}' has no particle prefab assigned.");
                return null;
            }
            
            // 親を指定せずにインスタンス化し、後から親を設定することでスケールの継承を防ぐ
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(_poolParent);

            VfxPlayer newPlayer = instance.AddComponent<VfxPlayer>();
            newPlayer.Initialize(ReturnToPool);
            return newPlayer;
        }

        private void ReturnToPool(VfxPlayer player)
        {
            player.gameObject.SetActive(false);

            int instanceIdToRemove = -1;
            foreach (var entry in _activeInstances)
            {
                if (entry.Value == player)
                {
                    instanceIdToRemove = entry.Key;
                    break;
                }
            }

            if (instanceIdToRemove != -1)
            {
                _activeInstances.Remove(instanceIdToRemove);
            }

            VfxDefinition definition = player.VfxDefinition;
            if (definition != null && _pool.ContainsKey(definition))
            {
                _pool[definition].Enqueue(player);
            }
            else
            {
                Debug.LogWarning($"Could not pool VfxPlayer for definition '{(definition != null ? definition.name : "null")}'. Destroying instance.");
                Destroy(player.gameObject);
            }
        }
    }
}
