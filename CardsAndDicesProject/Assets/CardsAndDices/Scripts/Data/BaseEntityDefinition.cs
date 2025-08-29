using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアビリティの持続時間、クールダウン、および使用制限ロジックの抽象基本クラス
    /// </summary>
    public abstract class BaseEntityDefinition : ScriptableObject
    {
        [Tooltip("A unique string ID for persistence purposes (e.g., save data).")]
        private string _id;

        /// <summary>
        /// Gets the unique string ID for persistence.
        /// </summary>
        public string Id => _id;
    }
}
