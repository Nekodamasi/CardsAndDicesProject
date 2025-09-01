using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 特定のVFXを、このGameObjectの位置で再生するトリガーとなるコンポーネント。
    /// </summary>
    public class VfxTrigger : MonoBehaviour
    {
        [Tooltip("再生するVFXの定義")]
        [SerializeField]
        private VfxDefinition _vfxDefinition;
        [Tooltip("rootGameObject")]
        [SerializeField]
        private GameObject _rootGameObject;

        [Tooltip("VFXManager")]
        [SerializeField]
        private VfxManager _vfxManager;

        /// <summary>
        /// 設定されたVFXを再生します。
        /// このメソッドは、UnityEventや他のスクリプトから呼び出すことを想定しています。
        /// </summary>
        public void Play()
        {
            if (_vfxDefinition == null)
            {
                Debug.LogWarning("VfxDefinitionがこのTriggerに設定されていません。", this);
                return;
            }

            if (_vfxManager == null)
            {
                Debug.LogError("VfxManagerのインスタンスが見つかりません。");
                return;
            }

            // VfxManagerに再生を要求する。再生完了を待つ必要はないため、Taskは破棄する。
            _ = _vfxManager.PlayVfxAsync(_vfxDefinition, _rootGameObject.transform.position, _rootGameObject.transform.rotation);
        }
    }
}
