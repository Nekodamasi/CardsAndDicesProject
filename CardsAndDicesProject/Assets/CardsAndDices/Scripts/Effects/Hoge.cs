using UnityEngine;

namespace CardsAndDices
{
    public class Hoge : MonoBehaviour
    {
        [Header("Display Root")]
        /// <summary>
        /// アニメーション内でコルーチンやUniTaskの実行基点となるMonoBehaviour。
        /// </summary>
        [SerializeField] public MultiRendererVisualController MultiRendererVisualController;
    }
}
