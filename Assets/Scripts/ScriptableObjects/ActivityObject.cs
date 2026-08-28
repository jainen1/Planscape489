using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "ActivityObject", menuName = "Scriptable Objects/ActivityObject")]
public class ActivityObject : StuffedScriptableObject
{
    public string title;
    public int length;
    public int fullStomachLength;
    public int happiness;
    public int money;

    [Header("Audio Settings")]
    public AudioClip sound;

    [Range(0.1f, 3.0f)]
    public float pitch = 1.0f;

    public class JData {
        public string version = "v1";
        public string translationKey;
        public string title;
        public int length;
        public int fullStomachLength;
        public int happiness;
        public int money;

        //public AudioClip sound;

        public float pitch;

        public ActivityObject LoadJData () {
            ActivityObject activityObject = ScriptableObject.CreateInstance<ActivityObject>();
            activityObject.name = translationKey;
            activityObject.title = title;
            activityObject.length = length;
            activityObject.fullStomachLength = fullStomachLength;
            activityObject.happiness = happiness;
            activityObject.money = money;

            //sound

            activityObject.pitch = pitch;
            return activityObject;
        }
    }

    public JData GetAsJData () {
        JData savedState = new JData();
        savedState.translationKey = translationKey;
        savedState.title = title;
        savedState.length = length;
        savedState.fullStomachLength = fullStomachLength;
        savedState.happiness = happiness;
        savedState.money = money;

        //sound

        savedState.pitch = pitch;
        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Activities");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".activity.json"), JsonUtility.ToJson(GetAsJData(), true));
    }
}