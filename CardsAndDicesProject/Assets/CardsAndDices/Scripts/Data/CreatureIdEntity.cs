using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Represents the unique identifier for any game entity (e.g., Creature, Item).
    /// This ScriptableObject asset itself acts as a type-safe ID.
    /// </summary>
    [CreateAssetMenu(fileName = "EntityDef_", menuName = "CardsAndDices/Definition/CreatureNameEntity")]
    public class CreatureIdEntity : BaseEntityDefinition
    {
        [SerializeField]
        [Tooltip("A unique string ID for persistence purposes (e.g., save data).")]
        private string _id;

        /// <summary>
        /// Gets the unique string ID for persistence.
        /// </summary>
        public string Id => _id;
    }
}
