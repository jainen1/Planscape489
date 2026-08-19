using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Campaign))]
public class CampaignEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Generate JSON")) { GlobalGameManager.SaveAllCampaignsToJson(); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}