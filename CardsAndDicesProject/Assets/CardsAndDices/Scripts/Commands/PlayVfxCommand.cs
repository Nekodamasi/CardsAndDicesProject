using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// VFXの再生を要求するコマンド。
    /// </summary>
    public struct PlayVfxCommand : ICommand
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

        public PlayVfxCommand(VfxDefinition vfxDefinition, Vector3 position, Quaternion rotation)
        {
            VfxDefinition = vfxDefinition;
            Position = position;
            Rotation = rotation;
        }

        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Execute()
        {
            // 実装なし
        }

        /// <summary>
        /// コマンドを元に戻します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Undo()
        {
            // 実装なし
        }

    }
}
