using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")]

public class MenuTheme : ScriptableObject
{
    public Color backgroundColor;

    public Color gridCellColor;

    public Color dailyTaskListColor;
    public Color weeklyTaskListColor;
    public Color bonusTaskListColor;

    public Color happinessColor;
    public Color moneyColor;

    public Color brightTextColor;
    public Color darkTextColor;

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
}