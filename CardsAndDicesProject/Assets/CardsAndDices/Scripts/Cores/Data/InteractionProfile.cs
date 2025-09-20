using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ゲームオブジェクトのインタラクションの振る舞いを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "InPro_", menuName = "CardsAndDices/Core/Data/InteractionProfile")]
    public class InteractionProfile : ScriptableObject
    {
        [Header("Interaction Flags")]
        [Tooltip("このオブジェクトをドラッグできるか")]
        public bool CanDrag = true;

        [Tooltip("このオブジェクトが他のオブジェクトのドロップ先になれるか")]
        public bool CanBeDropTarget = true;

        [Tooltip("このオブジェクトにホバーできるか")]
        public bool CanHover = true;

        [Tooltip("このオブジェクトをクリックできるか")]
        public bool CanClick = true;
    }
}
