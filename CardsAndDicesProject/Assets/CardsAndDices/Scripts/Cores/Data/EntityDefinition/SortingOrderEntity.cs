using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SortGroupのOrderやSortingControllerへの設定値として使用するソート順を管理する
    /// </summary>
    [CreateAssetMenu(fileName = "SortOdr_", menuName = "CardsAndDices/Cores/Data/EntityDefinition/SortingOrderEntity")]
    public class SortingOrderEntity : BaseEntityDefinition
    {
        [SerializeField] private int _orderValue;
        public int OderValue => _orderValue;
    }
}
