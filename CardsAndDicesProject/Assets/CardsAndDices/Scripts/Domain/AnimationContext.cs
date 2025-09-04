using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// アニメーション戦略に渡すためのコンテキスト情報。
    /// アニメーションに必要なコンポーネントへの参照を集約します。
    /// </summary>
    public class AnimationContext : MonoBehaviour
    {
        [Header("Display Root")]
        /// <summary>
        /// アニメーション内でコルーチンやUniTaskの実行基点となるMonoBehaviour。
        /// </summary>
        [SerializeField] public MultiRendererVisualController MultiRendererVisualController;

        /// <summary>
        /// アニメーション対象のTransform。
        /// </summary>
        [SerializeField] public Transform TargetTransform;

        /// <summary>
        /// アニメーション対象のSpriteRenderer。
        /// </summary>
        [SerializeField] public BaseSpriteView SpriteView;

        /// <summary>
        /// シェーダープロパティを効率的に変更するためのMaterialPropertyBlock。
        /// </summary>
        [SerializeField] public MaterialPropertyBlock MaterialPropertyBlock;

        /// <summary>
        /// アニメーションのターゲット位置。主に移動アニメーションで使用されます。
        /// </summary>
        [SerializeField] public Vector3 TargetPosition;

        /// <summary>
        /// 指定された色の明るさを増加させた新しい色を取得します。
        /// </summary>
        /// <param name="baseColor">基準となる色</param>
        /// <param name="increase">増加させる明るさの量</param>
        /// <returns>明るさを増加させた色</returns>
        public Color GetBrightenedColor(Color baseColor, float increase)
        {
            return new Color(
                Mathf.Clamp01(baseColor.r + increase),
                Mathf.Clamp01(baseColor.g + increase),
                Mathf.Clamp01(baseColor.b + increase),
                baseColor.a
            );
        }
    }
}
