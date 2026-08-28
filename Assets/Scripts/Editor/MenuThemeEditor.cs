using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuTheme))]
public class MenuThemeEditor : Editor {
    public override void OnInspectorGUI() {
        if(EditorApplication.isPlaying && GUILayout.Button("Reload & Update Theme")) {
            GlobalGameManager.LoadThemes();
            GlobalGameManager.SendThemeUpdate();
        }
        if(EditorApplication.isPlaying && GUILayout.Button("Generate, Reload & Update Theme")) {
            (target as MenuTheme).Save();
            GlobalGameManager.LoadThemes();
            GlobalGameManager.SendThemeUpdate();
        }
        if(GUILayout.Button("Generate Single JSON")) { 
            (target as MenuTheme).Save();
            Debug.Log("Saved theme as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<MenuTheme>("Themes"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}