using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")]

public class MenuTheme : ScriptableObject
{
    public TMPro.TMP_FontAsset mainFont;
    public float mainFontSizeScale = 1;

    public TMPro.TMP_FontAsset timerFont;
    public float timerFontSizeScale = 1;

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

    public Color resourceBarBackgroundColor;

    public Color happinessColor;
    public Color happinessChangeColor;
    public Color happinessOverflowColor;
    public Color happinessOverflowChangeColor;

    public Color moneyColor;
    public Color moneyChangeColor;
    public Color moneyOverflowColor;
    public Color moneyOverflowChangeColor;
    public Color moneyOverflow2Color;
    public Color moneyOverflow2ChangeColor;
}