using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Campaign))]
public class CampaignEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Generate Single JSON")) {
            (target as Campaign).Save();
            Debug.Log("Saved campaign as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<Campaign>("Campaigns"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}