using System.Collections.Generic; // Listを使用するため追加
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーステータスアイコンデータ
    /// </summary>
    [System.Serializable] // Unityエディタで表示・編集可能にするため
    public class CreatureStatusIconData
    {
        public EffectTargetType EffectTargetType;
        public SharedIconElementTypeEntity _sharedIconElementTypeEntity;
    }
}
