using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Week))]
public class WeekEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Generate JSON")) { GlobalGameManager.SaveAllWeeksToJson(); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}