using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Week))]
public class WeekEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Generate Single JSON")) {
            (target as Week).Save();
            Debug.Log("Saved week as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<Week>("Weeks"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}