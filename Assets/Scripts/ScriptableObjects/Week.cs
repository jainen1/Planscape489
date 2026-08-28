using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : StuffedScriptableObject {
    public MusicType musicType = MusicType.Calm;
    public GameplayType gameplayType = GameplayType.Calendar;

    public enum MusicType { Calm, Tense, SuperTense }
    public enum GameplayType { Calendar, Invaders, Zombies }

    [Header("Details")]
    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public string[] days = new string[7];
    public int hoursPerDay = 17;
    public int dayStartHour = 6;

    [Serializable]
    public class ResourceBarValues {
        public float min; public float max;

        [Serializable]
        public class Collection {
            public float startingValue;
            public ResourceBarValues[] resourceBars = new ResourceBarValues[0];
        }
    }
    public ResourceBarValues.Collection[] resourceBars;

    [Header("Activities")]
    public ActivityWithTime[] fixedActivities;
    [Serializable] public class ActivityWithTime { public ActivityObject activity; public Vector2 time; }
    public ActivityWithCount[] requiredTasks;
    public ActivityWithCount[] bonusTasks;
    [Serializable] public class ActivityWithCount { public ActivityObject activity; public int count; }

    [Header("Events")]
    //public float randomEventChance;
    public EventWithTime[] fixedEvents;
    [Serializable] public class EventWithTime { public EventObject eventObject; public Vector2 time; }

    [Header("Tutorial")]
    public TutorialContent[] tutorialContent;
    public enum TutorialContentType { Tutorial, Dialogue }
    [Serializable] public class TutorialContent { public TutorialContentType type; [TextArea(1, 16)] public string text; }

    public class JData {
        public string version = "v1";
        public string translationKey;
        public int musicType;
        public int gameplayType;

        public float timeHandSpeed;

        public float firstPreparationTime;
        public float dailyPreparationTime;

        public string[] days;
        public int hoursPerDay;
        public int dayStartHour;

        public ResourceBarValues.Collection[] resourceBarValues;
        [Serializable] public class ResourceBarValues {
            public float min; public float max;
            [Serializable] public class Collection {
                public float startingValue;
                public ResourceBarValues[] resourceBars;
            }
        }

        public ActivityWithTime[] fixedActivities;
        [Serializable] public class ActivityWithTime { public string activity; public Vector2 time; }
        public ActivityWithCount[] requiredTasks;
        public ActivityWithCount[] bonusTasks;
        [Serializable] public class ActivityWithCount { public string activity; public int count; }

        public EventWithTime[] fixedEvents;
        [Serializable] public class EventWithTime { public string eventObject; public Vector2 time; }

        public TutorialContent[] tutorialContent;
        [Serializable] public class TutorialContent { public int type; public string text; }

        public Week LoadJData () {
            Week week = ScriptableObject.CreateInstance<Week>();
            week.name = translationKey;
            week.translationKey = translationKey;
            week.musicType = (MusicType) musicType;
            week.gameplayType = (GameplayType) gameplayType;
            week.timeHandSpeed = timeHandSpeed;
            week.firstPreparationTime = firstPreparationTime;
            week.dailyPreparationTime = dailyPreparationTime;

            week.days = days;

            week.hoursPerDay = hoursPerDay;
            week.dayStartHour = dayStartHour;

            week.resourceBars = new Week.ResourceBarValues.Collection[resourceBarValues.Length];
            if(resourceBarValues.Length > 0) {
                for(int i = 0; i < resourceBarValues.Length; i++) {
                    Week.ResourceBarValues.Collection resourceBarValueCollection = new Week.ResourceBarValues.Collection();
                    resourceBarValueCollection.startingValue = resourceBarValues[i].startingValue;
                    resourceBarValueCollection.resourceBars = new Week.ResourceBarValues[resourceBarValues[i].resourceBars.Length];
                    if(resourceBarValues[i].resourceBars.Length > 0) {
                        for(int j = 0; j < resourceBarValues[i].resourceBars.Length; j++) {
                            Week.ResourceBarValues resourceBarValueSet = new Week.ResourceBarValues();
                            resourceBarValueSet.min = resourceBarValues[i].resourceBars[j].min;
                            resourceBarValueSet.max = resourceBarValues[i].resourceBars[j].max;
                            resourceBarValueCollection.resourceBars[j] = resourceBarValueSet;
                        }
                    }
                    week.resourceBars[i] = resourceBarValueCollection;
                }
            }

            week.fixedActivities = new Week.ActivityWithTime[fixedActivities.Length];
            if(fixedActivities.Length > 0) {
                for(int i = 0; i < fixedActivities.Length; i++) {
                    Week.ActivityWithTime obj = new Week.ActivityWithTime();
                    ActivityObject newActivity = JsonUtility.FromJson<ActivityObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", fixedActivities[i].activity + ".activity.json"))).LoadJData();
                    newActivity.name = newActivity.translationKey;
                    obj.activity = newActivity;
                    obj.time = fixedActivities[i].time;
                    week.fixedActivities[i] = obj;
                }
            }

            week.requiredTasks = new Week.ActivityWithCount[requiredTasks.Length];
            if(requiredTasks.Length > 0) {
                for(int i = 0; i < requiredTasks.Length; i++) {
                    Week.ActivityWithCount obj = new Week.ActivityWithCount();
                    ActivityObject newActivity = JsonUtility.FromJson<ActivityObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", requiredTasks[i].activity + ".activity.json"))).LoadJData();
                    newActivity.name = newActivity.translationKey;
                    obj.activity = newActivity;
                    obj.count = requiredTasks[i].count;
                    week.requiredTasks[i] = obj;
                }
            }

            week.bonusTasks = new Week.ActivityWithCount[bonusTasks.Length];
            if(bonusTasks.Length > 0) {
                for(int i = 0; i < bonusTasks.Length; i++) {
                    Week.ActivityWithCount obj = new Week.ActivityWithCount();
                    ActivityObject newActivity = JsonUtility.FromJson<ActivityObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", bonusTasks[i].activity + ".activity.json"))).LoadJData();
                    newActivity.name = newActivity.translationKey;
                    obj.activity = newActivity;
                    obj.count = bonusTasks[i].count;
                    week.bonusTasks[i] = obj;
                }
            }

            week.fixedEvents = new Week.EventWithTime[fixedEvents.Length];
            if(fixedEvents.Length > 0) {
                for(int i = 0; i < fixedEvents.Length; i++) {
                    Week.EventWithTime obj = new Week.EventWithTime();
                    EventObject newEvent = JsonUtility.FromJson<EventObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", fixedEvents[i].eventObject + ".event.json"))).LoadJData();
                    newEvent.name = newEvent.translationKey;
                    obj.eventObject = newEvent;
                    obj.time = fixedEvents[i].time;
                    week.fixedEvents[i] = obj;
                }
            }

            week.tutorialContent = new Week.TutorialContent[tutorialContent.Length];
            if(tutorialContent.Length > 0) {
                for(int i = 0; i < tutorialContent.Length; i++) {
                    Week.TutorialContent obj = new Week.TutorialContent();
                    obj.type = (TutorialContentType) tutorialContent[i].type;
                    obj.text = tutorialContent[i].text;
                    week.tutorialContent[i] = obj;
                }
            }

            return week;
        }
    }

    public JData GetAsJData () {
        JData savedState = new JData();
        savedState.translationKey = translationKey;
        savedState.musicType = (int) musicType;
        savedState.gameplayType = (int) gameplayType;
        savedState.timeHandSpeed = timeHandSpeed;
        savedState.firstPreparationTime = firstPreparationTime;
        savedState.dailyPreparationTime = dailyPreparationTime;

        savedState.days = days;

        savedState.hoursPerDay= hoursPerDay;
        savedState.dayStartHour = dayStartHour;

        savedState.resourceBarValues = new JData.ResourceBarValues.Collection[resourceBars.Length];
        if(resourceBars.Length > 0) {
            for(int i = 0; i < resourceBars.Length; i++) {
                JData.ResourceBarValues.Collection resourceBarValueCollection = new JData.ResourceBarValues.Collection();
                resourceBarValueCollection.startingValue = resourceBars[i].startingValue;
                resourceBarValueCollection.resourceBars = new JData.ResourceBarValues[resourceBars[i].resourceBars.Length];
                if(resourceBars[i].resourceBars.Length > 0) {
                    for(int j = 0; j < resourceBars[i].resourceBars.Length; j++) {
                        JData.ResourceBarValues resourceBarValueSet = new JData.ResourceBarValues();
                        resourceBarValueSet.min = resourceBars[i].resourceBars[j].min;
                        resourceBarValueSet.max = resourceBars[i].resourceBars[j].max;
                        resourceBarValueCollection.resourceBars[j] = resourceBarValueSet;
                    }
                }
                savedState.resourceBarValues[i] = resourceBarValueCollection;
            }
        }

        savedState.fixedActivities = new JData.ActivityWithTime[fixedActivities.Length];
        if(fixedActivities.Length > 0) {
            for(int i = 0; i < fixedActivities.Length; i++) {
                JData.ActivityWithTime obj = new JData.ActivityWithTime();
                obj.activity = Path.Combine("PlanscapeGenerated", "Activities", fixedActivities[i].activity.name);
                obj.time = fixedActivities[i].time;
                savedState.fixedActivities[i] = obj;
            }
        }

        savedState.requiredTasks = new JData.ActivityWithCount[requiredTasks.Length];
        if(requiredTasks.Length > 0) {
            for(int i = 0; i < requiredTasks.Length; i++) {
                JData.ActivityWithCount obj = new JData.ActivityWithCount();
                obj.activity = Path.Combine("PlanscapeGenerated", "Activities", requiredTasks[i].activity.name);
                obj.count = requiredTasks[i].count;
                savedState.requiredTasks[i] = obj;
            }
        }

        savedState.bonusTasks = new JData.ActivityWithCount[bonusTasks.Length];
        if(bonusTasks.Length > 0) {
            for(int i = 0; i < bonusTasks.Length; i++) {
                JData.ActivityWithCount obj = new JData.ActivityWithCount();
                obj.activity = Path.Combine("PlanscapeGenerated", "Activities", bonusTasks[i].activity.name);
                obj.count = bonusTasks[i].count;
                savedState.bonusTasks[i] = obj;
            }
        }

        savedState.fixedEvents = new JData.EventWithTime[fixedEvents.Length];
        if(fixedEvents.Length > 0) {
            for(int i = 0; i < fixedEvents.Length; i++) {
                JData.EventWithTime obj = new JData.EventWithTime();
                obj.eventObject = Path.Combine("PlanscapeGenerated", "Events", fixedEvents[i].eventObject.name);
                obj.time = fixedEvents[i].time;
                savedState.fixedEvents[i] = obj;
            }
        }

        savedState.tutorialContent = new JData.TutorialContent[tutorialContent.Length];
        if(tutorialContent.Length > 0) {
            for(int i = 0; i < tutorialContent.Length; i++) {
                JData.TutorialContent obj = new JData.TutorialContent();
                obj.type = (int) tutorialContent[i].type;
                obj.text = tutorialContent[i].text;
                savedState.tutorialContent[i] = obj;
            }
        }

        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Weeks");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".week.json"), JsonUtility.ToJson(GetAsJData(), true));
    }
}