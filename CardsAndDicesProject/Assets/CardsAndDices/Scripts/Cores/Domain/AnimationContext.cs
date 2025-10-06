using UnityEngine;
using VContainer;
namespace CardsAndDices
{
    /// <summary>
    /// アニメーション戦略に渡すためのコンテキスト情報。
    /// アニメーションに必要なコンポーネントへの参照を集約します。
    /// </summary>
    public class AnimationContext : MonoBehaviour
    {
        [Header("Component")]
        /// <summary>
        /// アニメーション内でコルーチンやUniTaskの実行基点となるMonoBehaviour。
        /// </summary>
        [SerializeField] public MultiRendererVisualController MultiRendererVisualController;

        /// <summary>
        /// アニメーション対象のTransform。
        /// </summary>
        [SerializeField] public Transform MoveTargetTransform;

        /// <summary>
        /// アニメーション対象のTransform。
        /// </summary>
        [SerializeField] public Transform ScaleTargetTransform;

        /// <summary>
        /// シェーダープロパティを効率的に変更するためのMaterialPropertyBlock。
        /// </summary>
        [SerializeField] public MaterialPropertyBlock MaterialPropertyBlock;

        /// <summary>
        /// CompositeObjectIdを取得するのに使用します
        /// </summary>
        [SerializeField] public IdentifiableGameObject IdentifiableGameObject;

        /// <summary>
        /// アニメーションのターゲット位置。主に移動アニメーションで使用されます。
        /// </summary>
        public Vector3 TargetPosition;

        /// <summary>
        /// アニメーションのホーム位置。主に移動アニメーションで使用されます。
        /// </summary>
        public Vector3 HomePosition;

        /// <summary>
        /// コマンドを発行するのに使用します
        /// </summary>
        public GameEventBus GameEventBus;

        /// <summary>
        /// vfxデータ
        /// </summary>
        public VfxDefinition VfxDefinition;

		[Inject]
		public void Construct(GameEventBus gameEventBus)
		{
			GameEventBus = gameEventBus;
        }

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
