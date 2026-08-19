using UnityEngine;

[CreateAssetMenu(fileName = "Lang", menuName = "Scriptable Objects/Language File")]
public class Lang : StuffedScriptableObject
{
    [Header("Translations")]
    public LangEntry[] translations;

    public class LangEntry {
        public string key;
        public string value;
    }
}