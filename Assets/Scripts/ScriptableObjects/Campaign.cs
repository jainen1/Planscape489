using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "Campaign", menuName = "Scriptable Objects/Campaign")]
public class Campaign : StuffedScriptableObject {
    public Sprite thumbnail;
    public Color accentColor;

    public Week[] weeks;

    [Header("Campaign Menu")]
    public string title; //temp as this will eventually be controlled by the translation key field
    [TextArea(3, 5)]
    public string description;
    public Difficulty difficulty;
    public enum Difficulty{ Easy, Medium, Hard }


    public class JData {
        public string version = "v1";
        public string translationKey;
        public string thumbnail;
        public Color32 accentColor;

        public string[] weeks;

        public string title;
        public string description;
        public int difficulty;

        public Campaign LoadJData() {
            Campaign campaign = ScriptableObject.CreateInstance<Campaign>();
            campaign.name = translationKey;
            campaign.translationKey = translationKey;
            campaign.thumbnail = JExtraUtility.LoadNewSprite(Path.Combine(Application.streamingAssetsPath, "ContentPacks", thumbnail + ".png"));
            campaign.accentColor = accentColor;
            campaign.weeks = new Week[weeks.Length];
            if(weeks.Length > 0) { for(int i = 0; i < weeks.Length; i++) {
                Week newWeek = JsonUtility.FromJson<Week.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", weeks[i] + ".week.json"))).LoadJData();
                newWeek.name = newWeek.translationKey;
                campaign.weeks[i] = newWeek;
            }}

            campaign.title = title;
            campaign.description = description;
            campaign.difficulty = (Difficulty) difficulty;
            return campaign;
        }
    }

    public JData GetAsJData () {
        JData savedState = new JData();
        savedState.translationKey = translationKey;
        savedState.thumbnail = Path.Combine("PlanscapeGenerated", "Images", thumbnail.texture.name); //starts in StreamingAssets/ContentPacks
        JExtraUtility.SaveSprite(thumbnail);
        savedState.accentColor = accentColor;        
        savedState.weeks = new string[weeks.Length];
        if(weeks.Length > 0) { for(int i = 0; i < weeks.Length; i++) { 
            savedState.weeks[i] = Path.Combine("PlanscapeGenerated", "Weeks", weeks[i].name);
        }}
        savedState.title = title;
        savedState.description = description;
        savedState.difficulty = (int) difficulty;
        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Campaigns");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".campaign.json"), JsonUtility.ToJson(GetAsJData(), true));
    }
}