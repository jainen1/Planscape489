using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")] [Serializable]
public class MenuTheme : ScriptableObject
{
    [Header("Fonts")]
    public TMPro.TMP_FontAsset mainFont;
    public float mainFontSizeScale = 1;
    public float mainCharacterSpacingScale = 1;
    public float mainLineSpacingScale = 1;

    public TMPro.TMP_FontAsset timerFont;
    public float timerFontSizeScale = 1;
    public float timerCharacterSpacingScale = 1;
    public float timerLineSpacingScale = 1;

    //public class Font

    [Header("Backgrounds")]

    public BackgroundLayer[] menuBackgroundLayers;
    public BackgroundLayer[] levelBackgroundLayers;

    [Serializable]
    public class BackgroundLayer {
        public Sprite sprite;
        public Color color;
        public Vector3 position = Vector3.zero;
        public Quaternion rotation;
        public Vector3 scale = Vector3.one;
    }

    [Header("Task Lists (0 = Required, 2 = Bonus, 1 = Unused)")]
    public TaskListColors[] taskListColors = new TaskListColors[2];

    [Serializable]
    public class TaskListColors {
        public Color mainColor;
        public Color itemColor;
        public Color scrollbarColor;
        public Color countColor;
        public Color[] taskColors = new Color[4];
    }

    [Header("Resource Bars (0 = Week, 1 = Happiness, 2 = Money)")]
    public ResourceBarColors.Collection[] resourceBarColors = new ResourceBarColors.Collection[3];
    public Color resourceBarBackgroundColor;

    [Serializable]
    public class ResourceBarColors {
        public Color fill = Color.green;
        public Color change = Color.white;

        [Serializable]
        public class Collection {
            public ResourceBarColors[] resourceBars = new ResourceBarColors[0];
        }
    }

    [Header("Other Colors")]

    public Color gridCellColor;
    public Color fixedGridCellColor;

    public Color brightTextColor;
    public Color darkTextColor;

    public Color menuButtonBackgroundColor;

    public Color fixedActivityColor;
    public Color fixedActivityBorderColor;

    public Color timeHandColor;
    public Color timeHandFastColor;

    public Color pauseButtonColor;
    public Color helpButtonColor;

    [Header("Music & SFX")]
    public AudioClip calmMusic;
    public AudioClip tenseMusic;
    public AudioClip superTenseMusic;

    public AudioClip buttonClick;
    public AudioClip[] clockTicking;
}