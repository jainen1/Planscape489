using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")]

public class MenuTheme : ScriptableObject
{
    [Header("Fonts")]
    public TMPro.TMP_FontAsset mainFont;
    public float mainFontSizeScale = 1;

    public TMPro.TMP_FontAsset timerFont;
    public float timerFontSizeScale = 1;

    [Header("Colors")]
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

    public Color dailyTaskSmallColor;
    public Color dailyTaskMediumColor;
    public Color dailyTaskLargeColor;

    public Color weeklyTaskSmallColor;
    public Color weeklyTaskMediumColor;
    public Color weeklyTaskLargeColor;

    public Color bonusTaskSmallColor;
    public Color bonusTaskMediumColor;
    public Color bonusTaskLargeColor;

    public Color fixedActivityColor;
    public Color fixedActivityBorderColor;

    public Color timeHandColor;

    public Color pauseButtonColor;
    public Color helpButtonColor;

    public Color resourceBarBackgroundColor;

    public ResourceBarColors[] happinessBars = new ResourceBarColors[2];
    public ResourceBarColors[] moneyBars = new ResourceBarColors[3];
}

[Serializable]
public class ResourceBarColors {
    public Color fill;
    public Color change = Color.white;
    public float min;
    public float max;
}