using UnityEngine;
using DG.Tweening;
using TMPro;

namespace CardsAndDices
{
    /// <summary>
    /// 複数のSpriteRendererとTextMeshProUGUIの視覚的プロパティ（透明度、色）を一括で制御するコンポーネント。
    /// 複雑なUI要素全体のフェードイン/アウトや色変更に利用します。
    /// </summary>
    public class MultiRendererVisualController : MonoBehaviour
    {
        [Header("Renderers to Control")]
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private TextMeshProUGUI[] _textMeshPros;
        [SerializeField] private MultiRendererVisualController[] _childControllers; // 階層的な制御用

        /// <summary>
        /// すべての登録されたレンダラーとテキストの透明度をTweenします。
        /// </summary>
        /// <param name="alpha">目標のアルファ値 (0.0f - 1.0f)。</param>
        /// <param name="duration">アニメーションの期間。</param>
        /// <returns>生成されたSequence。</returns>
        public Sequence FadeToAlpha(float alpha, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            // SpriteRendererの透明度をTween
            if (_spriteRenderers != null)
            {
                foreach (SpriteRenderer sr in _spriteRenderers)
                {
                    if (sr != null) sequence.Join(sr.DOFade(alpha, duration));
                }
            }

            // TextMeshProUGUIの透明度をTween
            if (_textMeshPros != null)
            {
                foreach (TextMeshProUGUI tmp in _textMeshPros)
                {
                    if (tmp != null) sequence.Join(tmp.DOFade(alpha, duration));
                }
            }

            // 子のMultiRendererVisualControllerの透明度をTween
            if (_childControllers != null)
            {
                foreach (MultiRendererVisualController childController in _childControllers)
                {
                    if (childController != null) sequence.Join(childController.FadeToAlpha(alpha, duration));
                }
            }

            return sequence;
        }

        /// <summary>
        /// すべての登録されたレンダラーとテキストの現在の透明度を設定します。
        /// </summary>
        /// <param name="alpha">設定するアルファ値 (0.0f - 1.0f)。</param>
        public void SetAlpha(float alpha)
        {
            // SpriteRendererの透明度を設定
            if (_spriteRenderers != null)
            {
                foreach (SpriteRenderer sr in _spriteRenderers)
                {
                    if (sr != null)
                    {
                        Color color = sr.color;
                        color.a = alpha;
                        sr.color = color;
                    }
                }
            }

            // TextMeshProUGUIの透明度を設定
            if (_textMeshPros != null)
            {
                foreach (TextMeshProUGUI tmp in _textMeshPros)
                {
                    if (tmp != null)
                    {
                        Color color = tmp.color;
                        color.a = alpha;
                        tmp.color = color;
                    }
                }
            }

            // 子のMultiRendererVisualControllerの透明度を設定
            if (_childControllers != null)
            {
                foreach (MultiRendererVisualController childController in _childControllers)
                {
                    if (childController != null) childController.SetAlpha(alpha);
                }
            }
        }

        /// <summary>
        /// すべての登録されたレンダラーとテキストの色をTweenします。
        /// </summary>
        /// <param name="targetColor">目標の色。</param>
        /// <param name="duration">アニメーションの期間。</param>
        /// <returns>生成されたSequence。</returns>
        public Sequence ColorTo(Color targetColor, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            // SpriteRendererの色をTween
            if (_spriteRenderers != null)
            {
                foreach (SpriteRenderer sr in _spriteRenderers)
                {
                    if (sr != null) sequence.Join(sr.DOColor(targetColor, duration));
                }
            }

            // TextMeshProUGUIの色をTween
            if (_textMeshPros != null)
            {
                foreach (TextMeshProUGUI tmp in _textMeshPros)
                {
                    if (tmp != null) sequence.Join(tmp.DOColor(targetColor, duration));
                }
            }

            // 子のMultiRendererVisualControllerの色をTween
            if (_childControllers != null)
            {
                foreach (MultiRendererVisualController childController in _childControllers)
                {
                    if (childController != null) sequence.Join(childController.ColorTo(targetColor, duration));
                }
            }

            return sequence;
        }

        /// <summary>
        /// すべての登録されたレンダラーとテキストの現在の色を設定します。
        /// </summary>
        /// <param name="targetColor">設定する色。</param>
        public void SetColor(Color targetColor)
        {
            // SpriteRendererの色を設定
            if (_spriteRenderers != null)
            {
                foreach (SpriteRenderer sr in _spriteRenderers)
                {
                    if (sr != null) sr.color = targetColor;
                }
            }

            // TextMeshProUGUIの色を設定
            if (_textMeshPros != null)
            {
                foreach (TextMeshProUGUI tmp in _textMeshPros)
                {
                    if (tmp != null) tmp.color = targetColor;
                }
            }

            // 子のMultiRendererVisualControllerの色を設定
            if (_childControllers != null)
            {
                foreach (MultiRendererVisualController childController in _childControllers)
                {
                    if (childController != null) childController.SetColor(targetColor);
                }
            }
        }

        /// <summary>
        /// 現在の色を取得します。複数のレンダラーがある場合は最初のレンダラーの色を返します。
        /// </summary>
        public Color GetColor()
        {
            if (_spriteRenderers != null && _spriteRenderers.Length > 0 && _spriteRenderers[0] != null)
            {
                return _spriteRenderers[0].color;
            }
            if (_textMeshPros != null && _textMeshPros.Length > 0 && _textMeshPros[0] != null)
            {
                return _textMeshPros[0].color;
            }
            return Color.white; // デフォルトの色を返す
        }
    }
}
