using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")]

public class MenuTheme : ScriptableObject
{
    public Color backgroundColor;

    public Color gridCellColor;
    public Color fixedGridCellColor;

    public Color brightTextColor;
    public Color darkTextColor;

    public Color dailyTaskListColor;
    public Color weeklyTaskListColor;
    public Color bonusTaskListColor;

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

    public Color happinessBackgroundColor;
    public Color happinessColor;
    public Color happinessChangeColor;
    public Color happinessOverflowColor;
    public Color happinessOverflowChangeColor;

    public Color moneyColor;
}