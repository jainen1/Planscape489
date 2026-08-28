using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageFile))]
public class LangEditor : Editor {
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Generate Single JSON")) { 
            (target as LanguageFile).Save();
            Debug.Log("Saved language file as JSON.");
        }
        if(GUILayout.Button("Generate All JSON")) { JExtraUtility.SaveObjectsToJson<LanguageFile>("Lang"); }
        DrawDefaultInspector();
        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}