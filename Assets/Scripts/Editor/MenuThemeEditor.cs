using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuTheme))]
public class MenuThemeEditor : Editor {
    public override void OnInspectorGUI() {
        if(EditorApplication.isPlaying && GUILayout.Button("Send Theme Update")) { GlobalGameManager.SendThemeUpdate(); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}