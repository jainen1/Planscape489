using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")] [Serializable]
public class MenuTheme : ScriptableObject
{
    [Header("Fonts")]
    public TMPro.TMP_FontAsset mainFont;
    public float mainFontSizeScale = 1;
    public float mainCharacterSpacingScale = 1;

    public TMPro.TMP_FontAsset timerFont;
    public float timerFontSizeScale = 1;
    public float timerCharacterSpacingScale = 1;

    [Header("Background")]
    public Sprite menuBackground;
    public Sprite levelBackground;

    [Header("Colors")]
    public Color menuBackgroundColor;
    public Color backgroundColor;

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

    public ResourceBarColors[] happinessBars = new ResourceBarColors[2];
    public ResourceBarColors[] moneyBars = new ResourceBarColors[3];
    public ResourceBarColors[] weekBars = new ResourceBarColors[1];

    [Header("Resource Bars (0 = Week, 1 = Happiness, 2 = Money)")]
    public ResourceBarColorsCollection[] resourceBarColors = new ResourceBarColorsCollection[0];
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