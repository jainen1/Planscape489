using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventObject))]
public class EventEditor : Editor {
    public override void OnInspectorGUI () {
        if(GUILayout.Button("Generate Single JSON")) {
            (target as EventObject).Save();
            Debug.Log("Saved event as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<EventObject>("Events"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}