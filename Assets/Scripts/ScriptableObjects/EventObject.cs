using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EventObject", menuName = "Scriptable Objects/EventObject")]
public class EventObject : StuffedScriptableObject
{
    public string title;
    [TextArea(1, 3)]
    public string description;

    public List<EventChoice> choices;

    [Serializable] public class EventChoice {
        public string description; public EventResult result;
    }

    [Serializable] public class EventResult {
        public int happinessChange;
        public int moneyChange;
        public List<Week.ActivityWithTime> addFixedActivities;
        public List<ActivityWithCountAndType> addActivities;
        public List<Week.EventWithTime> addEvents;
    }

    [Serializable] public class ActivityWithCountAndType {
        public Week.ActivityWithCount activityWithCount;
        public Activity.Type activityType;
    }

    public class JData {
        public string version = "v1";
        public string translationKey;
        public string title;
        public string description;

        public List<EventChoice> choices;

        [Serializable] public class EventChoice {
            public string description; public EventResult result;
        }

        [Serializable] public class EventResult {
            public int happinessChange;
            public int moneyChange;
            public List<Week.JData.ActivityWithTime> addFixedActivities;
            public List<ActivityWithCountAndType> addActivities;
            public List<Week.JData.EventWithTime> addEvents;
        }

        [Serializable] public class ActivityWithCountAndType {
            public Week.JData.ActivityWithCount activityWithCount;
            public Activity.Type activityType;
        }

        public EventObject LoadJData () {
            EventObject eventObject = ScriptableObject.CreateInstance<EventObject>();
            eventObject.name = translationKey;
            eventObject.title = title;
            eventObject.description = description;
            eventObject.choices = new List<EventObject.EventChoice>();
            if(choices.Count > 0) {
                for(int i = 0; i < choices.Count; i++) {
                    EventObject.EventChoice choice = new EventObject.EventChoice();
                    choice.description = choices[i].description;

                    EventObject.EventResult result = new EventObject.EventResult();
                    EventResult sourceResult = choices[i].result;

                    result.happinessChange = sourceResult.happinessChange;
                    result.moneyChange = sourceResult.moneyChange;

                    List<Week.ActivityWithTime> fixedActivities = new List<Week.ActivityWithTime>();
                    if(sourceResult.addFixedActivities.Count > 0) {
                        for(int j = 0; j < sourceResult.addFixedActivities.Count; j++) {
                            Week.ActivityWithTime stuffedActivity = new Week.ActivityWithTime();

                            ActivityObject activityItself = JsonUtility.FromJson<ActivityObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", sourceResult.addFixedActivities[j].activity + ".activity.json"))).LoadJData();
                            activityItself.name = activityItself.translationKey;
                            stuffedActivity.activity = activityItself;

                            stuffedActivity.time = sourceResult.addFixedActivities[j].time;
                            fixedActivities.Add(stuffedActivity);
                        }
                    }
                    result.addFixedActivities = fixedActivities;

                    List<EventObject.ActivityWithCountAndType> activities = new List<EventObject.ActivityWithCountAndType>();
                    if(sourceResult.addActivities.Count > 0) {
                        for(int j = 0; j < sourceResult.addActivities.Count; j++) {
                            EventObject.ActivityWithCountAndType stuffedActivity = new EventObject.ActivityWithCountAndType();
                            Week.ActivityWithCount lessStuffedActivity = new Week.ActivityWithCount();

                            ActivityObject activityItself = JsonUtility.FromJson<ActivityObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", sourceResult.addActivities[j].activityWithCount.activity + ".activity.json"))).LoadJData();
                            activityItself.name = activityItself.translationKey;
                            lessStuffedActivity.activity = activityItself;

                            lessStuffedActivity.count = sourceResult.addActivities[j].activityWithCount.count;
                            stuffedActivity.activityWithCount = lessStuffedActivity;

                            stuffedActivity.activityType = sourceResult.addActivities[j].activityType;
                            activities.Add(stuffedActivity);
                        }
                    }
                    result.addActivities = activities;

                    List<Week.EventWithTime> events = new List<Week.EventWithTime>();
                    if(sourceResult.addEvents.Count > 0) {
                        for(int j = 0; j < sourceResult.addEvents.Count; j++) {
                            Week.EventWithTime stuffedEvent = new Week.EventWithTime();

                            EventObject eventItself = JsonUtility.FromJson<EventObject.JData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ContentPacks", sourceResult.addEvents[j].eventObject + ".event.json"))).LoadJData();
                            eventItself.name = eventItself.translationKey;
                            stuffedEvent.eventObject = eventItself;

                            stuffedEvent.time = sourceResult.addEvents[j].time;
                            events.Add(stuffedEvent);
                        }
                    }
                    result.addEvents = events;

                    choice.result = result;
                    eventObject.choices.Add(choice); //might throw an error
                }
            }
            return eventObject;
        }
    }

    public JData GetAsJData () {
        JData savedState = new JData();
        savedState.title = title;
        savedState.description = description;
        savedState.choices = new List<JData.EventChoice>();
        if(choices.Count > 0) {
            for(int i = 0; i < choices.Count; i++) {
                JData.EventChoice newChoice = new JData.EventChoice();
                Debug.Log("Creating new EventChoice (index " + i + ")");
                newChoice.description = choices[i].description;

                JData.EventResult newResult = new JData.EventResult();
                EventResult sourceResult = choices[i].result;

                newResult.happinessChange = sourceResult.happinessChange;
                newResult.moneyChange = sourceResult.moneyChange;

                List<Week.JData.ActivityWithTime> newFixedActivities = new List<Week.JData.ActivityWithTime>();
                if(sourceResult.addFixedActivities.Count > 0) {
                    for(int j = 0; j < sourceResult.addFixedActivities.Count; j++) {
                        Week.JData.ActivityWithTime stuffedActivity = new Week.JData.ActivityWithTime();
                        stuffedActivity.activity = Path.Combine("PlanscapeGenerated", "Activities", sourceResult.addFixedActivities[j].activity.name);
                        stuffedActivity.time = sourceResult.addFixedActivities[j].time;
                        newFixedActivities.Add(stuffedActivity);
                    }
                }
                newResult.addFixedActivities = newFixedActivities;

                List<JData.ActivityWithCountAndType> newActivities = new List<JData.ActivityWithCountAndType>();
                if(sourceResult.addActivities.Count > 0) {
                    for(int j = 0; j < sourceResult.addActivities.Count; j++) {
                        JData.ActivityWithCountAndType stuffedActivity = new JData.ActivityWithCountAndType();
                        Week.JData.ActivityWithCount lessStuffedActivity = new Week.JData.ActivityWithCount();
                        lessStuffedActivity.activity = Path.Combine("PlanscapeGenerated", "Activities", sourceResult.addActivities[i].activityWithCount.activity.name);
                        lessStuffedActivity.count = sourceResult.addActivities[j].activityWithCount.count;

                        stuffedActivity.activityWithCount = lessStuffedActivity;
                        stuffedActivity.activityType = sourceResult.addActivities[j].activityType;
                        newActivities.Add(stuffedActivity);
                    }
                }
                newResult.addActivities = newActivities;

                List<Week.JData.EventWithTime> newEvents = new List<Week.JData.EventWithTime>();
                if(sourceResult.addEvents.Count > 0) {
                    for(int j = 0; j < sourceResult.addEvents.Count; j++) {
                        Week.JData.EventWithTime stuffedEvent = new Week.JData.EventWithTime();
                        stuffedEvent.eventObject = Path.Combine("PlanscapeGenerated", "Events", sourceResult.addEvents[j].eventObject.name);
                        stuffedEvent.time = sourceResult.addEvents[j].time;
                        newEvents.Add(stuffedEvent);
                    }
                }
                newResult.addEvents = newEvents;

                newChoice.result = newResult;
                savedState.choices.Add(newChoice);
            }
        }
        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Events");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".event.json"), JsonUtility.ToJson(GetAsJData(), true));
    }
}