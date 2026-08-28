using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "LanguageFile", menuName = "Scriptable Objects/Language File")]
public class LanguageFile : StuffedScriptableObject
{
    [Serializable] public class Entry { public string key; public string value; }
    public Entry[] translations;

    public class JData {
        public string translationKey;
        public Entry[] translations;

        public LanguageFile LoadData () {
            LanguageFile lang = ScriptableObject.CreateInstance<LanguageFile>();
            lang.name = translationKey;
            lang.translationKey = translationKey;
            lang.translations = new Entry[translations.Length];
            if(translations.Length > 0) {
                for(int i = 0; i < translations.Length; i++) {
                    Entry entry = new Entry();
                    entry.key = translations[i].key;
                    entry.value = translations[i].value;
                    lang.translations[i] = entry;
                }
            }
            return lang;
        }
    }

    public JData GetJData () {
        JData savedState = new JData();
        savedState.translationKey = translationKey;
        savedState.translations = new Entry[translations.Length];
        if(translations.Length > 0) {
            for(int i = 0; i < translations.Length; i++) {
                Entry entry = new Entry();
                entry.key = translations[i].key;
                entry.value = translations[i].value;
                savedState.translations[i] = entry;
            }
        }
        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Lang");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".lang.json"), JsonUtility.ToJson(GetJData(), true));
    }
}