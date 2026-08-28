using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActivityObject))]
public class ActivityEditor : Editor {
    public override void OnInspectorGUI () {
        if(GUILayout.Button("Generate Single JSON")) {
            (target as ActivityObject).Save();
            Debug.Log("Saved activity as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<ActivityObject>("Activities"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}