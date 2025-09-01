using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// VFXの再生を要求するコマンド。
    /// </summary>
    public struct PlayVfxCommand
    {
        /// <summary>
        /// 再生するVFXの定義。
        /// </summary>
        public VfxDefinition VfxDefinition;

        /// <summary>
        /// 再生する位置。
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// 再生する回転。
        /// </summary>
        public Quaternion Rotation;
    }
}
