using UnityEngine;
using UnityEngine.Rendering;

namespace CardsAndDice
{
    /// <summary>
    /// 複数のSortingGroup、Canvas、および子階層のSpriteLayerControllerの描画順序（sortingOrder）を一括で制御するコンポーネントです。
    /// </summary>
    public class SpriteLayerController : MonoBehaviour
    {
        [Header("Target Components")]
        [Tooltip("制御対象のSortingGroupの配列")]
        [SerializeField] private SortingGroup[] _sortingGroups;

        [Tooltip("制御対象のCanvasの配列")]
        [SerializeField] private Canvas[] _canvases;

        [Tooltip("制御対象の子SpriteLayerControllerの配列")]
        [SerializeField] private SpriteLayerController[] _childControllers;

        /// <summary>
        /// 登録されたすべての要素のsortingOrderを指定された値に設定します。
        /// 子のSpriteLayerControllerにも再帰的に適用されます。
        /// </summary>
        /// <param name="order">設定する描画順序の値。</param>
        public void SetOrderInLayer(int order)
        {
            // SortingGroupのsortingOrderを設定
            if (_sortingGroups != null)
            {
                foreach (var group in _sortingGroups)
                {
                    if (group != null)
                    {
                        group.sortingOrder = order;
                    }
                }
            }

            // CanvasのsortingOrderを設定
            if (_canvases != null)
            {
                foreach (var canvas in _canvases)
                {
                    if (canvas != null)
                    {
                        canvas.sortingOrder = order;
                    }
                }
            }

            // 子のSpriteLayerControllerのSetOrderInLayerを再帰的に呼び出し
            if (_childControllers != null)
            {
                foreach (var controller in _childControllers)
                {
                    if (controller != null)
                    {
                        controller.SetOrderInLayer(order);
                    }
                }
            }
        }
    }
}
