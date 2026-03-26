using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuObject : MonoBehaviour
{
    [SerializeField] private MenuObjectType type;
    [SerializeField] private ThemeTarget target;

    [HideInInspector] public Color color = Color.red;

    void OnEnable() { GlobalGameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= UpdateMenuObject; }

    public void UpdateMenuObject() {
        MenuTheme menuTheme = GlobalGameManager.Instance.GetMenuTheme();

        switch(type) {
            case MenuObjectType.Background: color = menuTheme.backgroundColor; break;

            case MenuObjectType.GridCell: color = gameObject.GetComponent<GridCell>().isFixed ? menuTheme.fixedGridCellColor : menuTheme.gridCellColor; break;

            case MenuObjectType.DailyTaskList: color = menuTheme.dailyTaskListColor; break;
            case MenuObjectType.DailyTaskListSecondary: color = menuTheme.dailyTaskListSecondaryColor; break;
            case MenuObjectType.WeeklyTaskList: color = menuTheme.weeklyTaskListColor; break;
            case MenuObjectType.WeeklyTaskListSecondary: color = menuTheme.weeklyTaskListSecondaryColor; break;
            case MenuObjectType.BonusTaskList: color = menuTheme.bonusTaskListColor; break;
            case MenuObjectType.BonusTaskListSecondary: color = menuTheme.bonusTaskListSecondaryColor; break;

            case MenuObjectType.TaskListCounter: color = menuTheme.taskListCounterColor; break;

            case MenuObjectType.ActivityPanel: color = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme); break;
            case MenuObjectType.ActivityShadowPanel: {
                Color temp = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme);
                temp.a = 0.7f;
                color = temp;
                break;
            }

            case MenuObjectType.FixedActivityBorder: color = menuTheme.fixedActivityBorderColor; break;
            case MenuObjectType.TimeHand: color = menuTheme.timeHandColor; break;

            case MenuObjectType.ActivityResource: color = ActivityResourceColor(GetActivityPanelColor(gameObject.transform.parent.transform.parent.transform.parent.GetComponent<ActivityInitializer>(), menuTheme)); break;

            case MenuObjectType.BrightText: color = menuTheme.brightTextColor; break;
            case MenuObjectType.DarkText: color = menuTheme.darkTextColor; break;

            case MenuObjectType.PauseButton: color = menuTheme.pauseButtonColor; break;
            case MenuObjectType.HelpButton: color = menuTheme.helpButtonColor; break;

            default: break;
        };

        switch(target) {
            case ThemeTarget.SpriteRenderer: gameObject.GetComponent<SpriteRenderer>().color = color; break;
            case ThemeTarget.Image: gameObject.GetComponent<Image>().color = color; break;
            case ThemeTarget.TextMeshPro: gameObject.GetComponent<TextMeshProUGUI>().color = color; break;
            default : break;
        }
    }

    private Color ActivityResourceColor(Color color) {
        float change = GetBrightOrDarkColor(color, 200)? 0.4f : -0.4f;
        float H;
        float S;
        float V;
        Color.RGBToHSV(color, out H, out S, out V);

        return Color.HSVToRGB(H, S, V + change);
    }

    private Color GetActivityPanelColor(ActivityInitializer activityInitializer, MenuTheme menuTheme) {
        if(activityInitializer != null) {
            if(activityInitializer.IsFixed()) {
                return menuTheme.fixedActivityColor;
            }
            if(activityInitializer.activity != null) {
                switch(activityInitializer.activity.length) {
                    case int n when n >= 4:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return menuTheme.dailyTaskLargeColor;
                            case ActivityType.Weekly: return menuTheme.weeklyTaskLargeColor;
                            case ActivityType.Bonus: return menuTheme.bonusTaskLargeColor;
                        }
                        break;
                    case int n when n >= 2:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return menuTheme.dailyTaskMediumColor;
                            case ActivityType.Weekly: return menuTheme.weeklyTaskMediumColor;
                            case ActivityType.Bonus: return menuTheme.bonusTaskMediumColor;
                        }
                        break;
                    default:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return menuTheme.dailyTaskSmallColor;
                            case ActivityType.Weekly: return menuTheme.weeklyTaskSmallColor;
                            case ActivityType.Bonus: return menuTheme.bonusTaskSmallColor;
                        }
                        break;
                }
            }
            
        } return Color.yellow;
    }

    public static bool GetBrightOrDarkColor(Color backgroundColor, int threshold) {
        float backgroundColorBrightness = (0.2126f * (backgroundColor.r * 255)) + (0.7152f * (backgroundColor.g * 255)) + (0.0722f * (backgroundColor.b * 255));
        bool brighter = (backgroundColorBrightness <= threshold);
        //Debug.Log("This object's brightness is " + backgroundColorBrightness + ", which is " + (brighter? "brighter" : "darker")  + " than the threshold of " + threshold + ".");
        return brighter; // 'true' means light, 'false' means dark
    }

    private enum ThemeTarget {
        SpriteRenderer,
        Image,
        TextMeshPro
    }

    public enum MenuObjectType {
        Background,

        GridCell,
        GridHeaderText,

        DailyTaskList,
        WeeklyTaskList,
        BonusTaskList,

        ActivityPanel,
        ActivityShadowPanel,
        ActivityResource,
        FixedActivityBorder,

        TimeHand,

        HappinessBackground,
        Happiness,
        HappinessChange,
        HappinessOverflow,
        HappinessOverflowChange,

        MoneyBackground,
        Money,
        MoneyChange,
        MoneyOverflow,
        MoneyOverflowChange,
        MoneyOverflow2,
        MoneyOverflow2Change,

        DailyTaskListSecondary,
        WeeklyTaskListSecondary,
        BonusTaskListSecondary,

        BrightText,
        DarkText,

        PauseButton,
        HelpButton,
        TaskListCounter
    }
}