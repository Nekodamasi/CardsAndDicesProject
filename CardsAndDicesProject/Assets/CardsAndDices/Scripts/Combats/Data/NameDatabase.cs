using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CardsAndDices
{
    /// <summary>
    /// A database that maps EntityDefinitions to their language-specific names and descriptions.
    /// </summary>
    [CreateAssetMenu(fileName = "NameDB_", menuName = "CardsAndDices/Database/NameDatabase")]
    public class NameDatabase : ScriptableObject
    {
        /// <summary>
        /// Holds the display name and description for an entity.
        /// </summary>
        [System.Serializable]
        public class NameData
        {
            public string DisplayName;
            public string Description;
        }

        /// <summary>
        /// Represents a single entry mapping an EntityDefinition to its NameData.
        /// </summary>
        [System.Serializable]
        public class Entry
        {
            public BaseEntityDefinition Definition;
            public NameData Data;
        }

        [SerializeField]
        private List<Entry> _entries = new List<Entry>();

        private Dictionary<BaseEntityDefinition, NameData> _database;

        /// <summary>
        /// Builds the internal dictionary for fast lookups.
        /// </summary>
        public void BuildDatabase()
        {
            _database = new Dictionary<BaseEntityDefinition, NameData>();
            foreach (var entry in _entries)
            {
                if (entry.Definition != null && !_database.ContainsKey(entry.Definition))
                {
                    _database.Add(entry.Definition, entry.Data);
                }
            }
        }

        /// <summary>
        /// Gets the NameData for a given EntityDefinition.
        /// </summary>
        /// <param name="definition">The entity definition to look up.</param>
        /// <returns>The corresponding NameData, or null if not found.</returns>
        public NameData GetNameData(BaseEntityDefinition definition)
        {
            if (_database == null || _database.Count != _entries.Count) // Rebuild if out of sync
            {
                BuildDatabase();
            }

            _database.TryGetValue(definition, out var nameData);
            return nameData;
        }

#if UNITY_EDITOR
        /// <summary>
        /// (Editor-only) Collects all EntityDefinition assets from a specified folder and adds them to the database.
        /// </summary>
        /// <param name="folderPath">The path to the folder, relative to the Assets folder.</param>
        public void CollectEntityDefinitionsFromFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            Undo.RecordObject(this, "Collect Entity Definitions");

            var existingDefinitions = new HashSet<BaseEntityDefinition>(_entries.Select(e => e.Definition));

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BaseEntityDefinition)}", new[] { folderPath });
            bool added = false;
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var definition = AssetDatabase.LoadAssetAtPath<BaseEntityDefinition>(assetPath);

                if (definition != null && !existingDefinitions.Contains(definition))
                {
                    _entries.Add(new Entry { Definition = definition, Data = new NameData() });
                    existingDefinitions.Add(definition);
                    added = true;
                }
            }

            if (added)
            {
                // Sort entries alphabetically by definition name
                _entries = _entries.OrderBy(e => e.Definition.name).ToList();
                EditorUtility.SetDirty(this);
                Debug.Log($"Added new definitions to {name}. Total entries: {_entries.Count}");
            }
            else
            {
                Debug.Log($"No new definitions found in {folderPath}.");
            }
        }
#endif
    }
}
