using UnityEngine;
using TMPro;

namespace CardsAndDices
{
    /// <summary>
    /// ステータスアイコンの画像やフォーマットなどの静的なデータを定義します。
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatusIconData", menuName = "CardsAndDices/Status Icon Data")]
    public class StatusIconData : ScriptableObject
    {
        /// <summary>
        /// アイコンとして表示されるスプライト。
        /// </summary>
        [Tooltip("アイコンとして表示されるスプライトId")]
        public string IconSpriteId;

        /// <summary>
        /// 値を表示するためのフォーマット文字列。{0}を数値のプレースホルダーとして使用します。
        /// </summary>
        [Tooltip("値を表示するためのフォーマット文字列。{0}を数値のプレースホルダーとして使用します。 ")]
        public string FormatString = "{0}";

        /// <summary>
        /// 数値テキストを表示するかどうか。
        /// </summary>
        [Tooltip("数値テキストを表示するかどうか。")]
        public bool ShowValue = true;

        /// <summary>
        /// このアイコンが表すステータスの種類。
        /// </summary>
        [Tooltip("このアイコンが表すステータスの種類。")]
        public EffectTargetType TargetType;
    }
}
