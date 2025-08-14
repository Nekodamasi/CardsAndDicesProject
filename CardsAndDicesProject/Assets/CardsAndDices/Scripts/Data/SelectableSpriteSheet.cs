using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// IDとスプライトのペアをコレクションとして保持するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "NewSelectableSpriteSheet", menuName = "CardsAndDices/SelectableSpriteSheet")]
    public class SelectableSpriteSheet : ScriptableObject
    {
        /// <summary>
        /// IDとスプライトのペアを定義するシリアライズ可能な構造体。
        /// </summary>
        [System.Serializable]
        public struct IdSpritePair
        {
            public string Id;
            public Sprite Sprite;
        }

        [SerializeField]
        private List<IdSpritePair> _sprites = new List<IdSpritePair>();

        // 実行時に高速な検索を行うためのキャッシュ
        private Dictionary<string, Sprite> _spriteMap;

        private void OnEnable()
        {
            // 起動時またはインスペクターでの値変更時にDictionaryを初期化し、データをキャッシュする
            // ToDictionaryは重複キーがあると例外を投げるため、重複を許容する場合は別の実装を検討
            _spriteMap = _sprites.ToDictionary(pair => pair.Id, pair => pair.Sprite);
        }

        /// <summary>
        /// 指定されたIDに対応するスプライトを取得します。
        /// </summary>
        /// <param name="id">取得したいスプライトのID。</param>
        /// <returns>対応するスプライト。見つからない場合はnullを返します。</returns>
        public Sprite GetSprite(string id)
        {
            if (string.IsNullOrEmpty(id) || _spriteMap == null)
            {
                return null;
            }

            _spriteMap.TryGetValue(id, out Sprite sprite);
            return sprite;
        }
    }
}