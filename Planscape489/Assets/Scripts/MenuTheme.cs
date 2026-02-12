using UnityEngine;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")]

public class MenuTheme : ScriptableObject
{
    public Color backgroundColor;

    public Color gridCellColor;
    public Color gridHeaderTextColor;

    public Color dailyTaskListColor;
    public Color dailyTaskTextColor;

    public Color weeklyTaskListColor;
    public Color weeklyTaskTextColor;

    public Color bonusTaskListColor;
    public Color bonusTaskTextColor;

    public Color happinessColor;
    public Color moneyColor;

    public Color fixedActivityColor;
    public Color fixedActivityBorderColor;
    public Color activityTextColor;
}
