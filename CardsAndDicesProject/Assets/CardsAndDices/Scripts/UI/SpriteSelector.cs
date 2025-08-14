using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SelectableSpriteSheetのデータに基づき、IDを指定してSpriteRendererの表示を切り替える汎用コンポーネント。
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSelector : MonoBehaviour
    {
        [Header("Data Source")]
        [SerializeField] private SelectableSpriteSheet _spriteSheet;

        // 表示を切り替える対象のSpriteRenderer
        [SerializeField] private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// 指定されたIDのスプライトをSpriteRendererに設定します。
        /// </summary>
        /// <param name="id">表示したいスプライトのID。</param>
        public void SelectSprite(string id)
        {
            if (_spriteSheet == null)
            {
                Debug.LogError($"SpriteSheet is not assigned in {gameObject.name}.", this);
                return;
            }

            Sprite sprite = _spriteSheet.GetSprite(id);
            if (sprite != null)
            {
                _spriteRenderer.sprite = sprite;
            }
            else
            {
                // 対応するスプライトが見つからない場合は警告を出す
                Debug.LogWarning($"Sprite with ID '{id}' not found in the sprite sheet '{_spriteSheet.name}'.", this);
            }
        }
    }
}