using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")] [Serializable]
public class MenuTheme : ScriptableObject
{
    [Header("Music")]
    public AudioClip music;

    [Header("Fonts")]
    public TMPro.TMP_FontAsset mainFont;
    public float mainFontSizeScale = 1;
    public float mainCharacterSpacingScale = 1;

    public TMPro.TMP_FontAsset timerFont;
    public float timerFontSizeScale = 1;
    public float timerCharacterSpacingScale = 1;

    [Header("Backgrounds")]

    public BackgroundLayer[] menuBackgroundLayers;
    public BackgroundLayer[] levelBackgroundLayers;

    [Header("Colors")]

    public Color gridCellColor;
    public Color fixedGridCellColor;

    public Color brightTextColor;
    public Color darkTextColor;

    public Color dailyTaskListColor;
    public Color dailyTaskListSecondaryColor;
    public Color weeklyTaskListColor;
    public Color weeklyTaskListSecondaryColor;
    public Color bonusTaskListColor;
    public Color bonusTaskListSecondaryColor;

    public Color taskListScrollbarColor;
    public Color taskListCounterColor;

    public Color requiredTask1Color;
    public Color requiredTask2Color;
    public Color requiredTask3Color;
    public Color requiredTask4Color;

    public Color bonusTask1Color;
    public Color bonusTask2Color;
    public Color bonusTask3Color;
    public Color bonusTask4Color;

    public Color fixedActivityColor;
    public Color fixedActivityBorderColor;

    public Color timeHandColor;
    public Color timeHandFastColor;

    public Color pauseButtonColor;
    public Color helpButtonColor;

    public Color resourceBarBackgroundColor;

    [Header("Resource Bars (0 = Week, 1 = Happiness, 2 = Money)")]
    public ResourceBarColorsCollection[] resourceBarColors = new ResourceBarColorsCollection[3];
}

[Serializable]
public class ResourceBarColorsCollection {
    public ResourceBarColors[] resourceBars = new ResourceBarColors[0];
}

[Serializable]
public class ResourceBarColors {
    public Color fill = Color.green;
    public Color change = Color.white;
}

[Serializable]
public class BackgroundLayer {
    public Sprite sprite;
    public Color color;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation;
    public Vector3 scale = Vector3.one;
}

[CustomEditor(typeof(MenuTheme))]
public class MyScriptEditor : Editor {
    public override void OnInspectorGUI() {
        if(EditorApplication.isPlaying && GUILayout.Button("Send Theme Update")) {
            GlobalGameManager.SendThemeUpdate();
        }

        DrawDefaultInspector();

        //myScript.doesntMatter = EditorGUILayout.Toggle("Hello World"); //Returns true when user clicks
    }
}