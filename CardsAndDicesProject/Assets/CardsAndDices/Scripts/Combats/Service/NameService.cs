using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Provides a service to resolve entity names based on a NameDatabase.
    /// </summary>
    [CreateAssetMenu(fileName = "NameService", menuName = "CardsAndDices/Service/NameService")]
    public class NameService : ScriptableObject, INameService
    {
        private NameDatabase _nameDatabase;

        /// <summary>
        /// Initializes the service with a specific name database.
        /// This should be called during the dependency injection setup (e.g., in GameLifetimeScope).
        /// </summary>
        /// <param name="database">The name database to use for lookups.</param>
        public void Initialize(NameDatabase database)
        {
            _nameDatabase = database;
            _nameDatabase.BuildDatabase();
        }

        /// <summary>
        /// Gets the display name for a given entity definition.
        /// </summary>
        /// <param name="entityDef">The entity definition.</param>
        /// <returns>The display name, or a default string if not found.</returns>
        public string GetDisplayName(BaseEntityDefinition entityDef)
        {
            if (_nameDatabase == null || entityDef == null)
            {
                return "[INVALID NAME]";
            }

            var nameData = _nameDatabase.GetNameData(entityDef);
            return nameData != null ? nameData.DisplayName : "[NAME NOT FOUND]";
        }

        /// <summary>
        /// Gets the description for a given entity definition.
        /// </summary>
        /// <param name="entityDef">The entity definition.</param>
        /// <returns>The description, or a default string if not found.</returns>
        public string GetDescription(BaseEntityDefinition entityDef)
        {
            if (_nameDatabase == null || entityDef == null)
            {
                return "[INVALID DESCRIPTION]";
            }

            var nameData = _nameDatabase.GetNameData(entityDef);
            return nameData != null ? nameData.Description : "[DESCRIPTION NOT FOUND]";
        }
    }
}
