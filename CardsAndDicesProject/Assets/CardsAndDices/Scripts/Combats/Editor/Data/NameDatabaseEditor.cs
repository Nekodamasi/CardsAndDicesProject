using UnityEditor;
using UnityEngine;
using System.IO;

namespace CardsAndDices
{
    /// <summary>
    /// Custom editor for the NameDatabase ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(NameDatabase))]
    public class NameDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Get the target object
            var nameDatabase = (NameDatabase)target;

            // Add a button to collect definitions
            if (GUILayout.Button("Collect Definitions from Folder"))
            {
                // Open a folder panel
                string path = EditorUtility.OpenFolderPanel("Select Folder with EntityDefinitions", "Assets", "");

                if (!string.IsNullOrEmpty(path))
                {
                    // Make the path relative to the Assets folder
                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        Debug.LogWarning("Please select a folder within the project's Assets directory.");
                        return;
                    }

                    // Call the method on the NameDatabase instance
                    nameDatabase.CollectEntityDefinitionsFromFolder(path);
                }
            }
        }
    }
}
