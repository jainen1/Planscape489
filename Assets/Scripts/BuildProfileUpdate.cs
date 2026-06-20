using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class BuildProfileUpdate
{
    // Automatically runs when Unity loads
    static BuildProfileUpdate()
    {
        UpdateBuildScenes();
    }

    [MenuItem("Tools/Refresh Build Scenes")]
    public static void UpdateBuildScenes()
    {
        // Find all scene files
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
        
        var newScenePaths = sceneGuids.Select(AssetDatabase.GUIDToAssetPath).Select(path => new EditorBuildSettingsScene(path, true)).ToArray();

        // Update build settings automatically
        EditorBuildSettings.scenes = newScenePaths;
    }
}