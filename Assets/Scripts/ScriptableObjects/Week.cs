using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Week", menuName = "Scriptable Objects/Week")]
public class Week : StuffedScriptableObject {
    public Utilities.MusicType musicType = Utilities.MusicType.Calm;
    public Utilities.GameplayType gameplayType = Utilities.GameplayType.Calendar;

    [Header("Details")]
    public float timeHandSpeed = 0.48f;

    public float firstPreparationTime = 60f;
    public float dailyPreparationTime = 15f;

    public int days = 7;
    public int hoursPerDay = 17;
    public int dayStartHour = 6;

    public Utilities.ResourceBarValues.Collection[] resourceBars;

    [Header("Activities")]
    public Utilities.ActivityWithTime[] fixedActivities;
    public Utilities.ActivityWithCount[] requiredTasks;
    public Utilities.ActivityWithCount[] bonusTasks;

    [Header("Events")]
    //public float randomEventChance;
    public Utilities.EventWithTime[] fixedEvents;

    [Header("Tutorial")]
    public Utilities.TutorialContent[] tutorialContent;

    public static class Utilities {
        [Serializable]
        public class ActivityWithTime {
            public ActivityObject activity;
            public Vector2 time;
        }

        [Serializable]
        public class ActivityWithCount {
            public ActivityObject activity;
            public int count;
        }

        [Serializable]
        public class EventWithTime {
            public EventObject eventObject;
            public Vector2 time;
        }

        [Serializable]
        public class ResourceBarValues {
            public float min;
            public float max;

            [Serializable]
            public class Collection {
                public float startingValue;
                public ResourceBarValues[] resourceBars = new ResourceBarValues[0];
            }
        }

        [Serializable]
        public class TutorialContent {
            public TutorialContentType type;
            [TextArea(1, 16)]
            public string text;
        }

        public enum MusicType {
            Calm,
            Tense,
            SuperTense
        }

        public enum GameplayType {
            Calendar,
            Invaders,
            Zombies
        }

        public enum TutorialContentType {
            Tutorial,
            Dialogue
        }
    }
}