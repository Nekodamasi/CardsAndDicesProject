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
    }
}
