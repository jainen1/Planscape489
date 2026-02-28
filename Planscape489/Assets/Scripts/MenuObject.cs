using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuObject : MonoBehaviour
{
    [SerializeField] private MenuObjectType type;
    [SerializeField] private ThemeTarget target;

    private LevelManager gameManager;

    [HideInInspector] public Color color = Color.red;

    void OnEnable() { LevelManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { LevelManager.OnUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<LevelManager>();

        switch(type) {
            case MenuObjectType.Background: color = gameManager.menuTheme.backgroundColor; break;

            case MenuObjectType.GridCell: color = gameObject.GetComponent<GridCell>().isFixed ? gameManager.menuTheme.fixedGridCellColor : gameManager.menuTheme.gridCellColor; break;
            //case MenuObjectType.GridHeaderText: color = GetBrightOrDarkTextColor(gameManager.menuTheme.gridCellColor, 128) ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor; isText = true; break;

            case MenuObjectType.DailyTaskList: color = gameManager.menuTheme.dailyTaskListColor; break;
            case MenuObjectType.DailyTaskListSecondary: color = gameManager.menuTheme.dailyTaskListSecondaryColor; break;
            case MenuObjectType.WeeklyTaskList: color = gameManager.menuTheme.weeklyTaskListColor; break;
            case MenuObjectType.WeeklyTaskListSecondary: color = gameManager.menuTheme.weeklyTaskListSecondaryColor; break;
            case MenuObjectType.BonusTaskList: color = gameManager.menuTheme.bonusTaskListColor; break;
            case MenuObjectType.BonusTaskListSecondary: color = gameManager.menuTheme.bonusTaskListSecondaryColor; break;

            case MenuObjectType.ActivityPanel: color = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>()); break;
            case MenuObjectType.ActivityShadowPanel: {
                Color temp = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>());
                temp.a = 0.7f;
                color = temp;
                break;
            }

            case MenuObjectType.FixedActivityBorder: color = gameManager.menuTheme.fixedActivityBorderColor; break;
            case MenuObjectType.TimeHand: color = gameManager.menuTheme.timeHandColor; break;

            case MenuObjectType.ActivityResource: color = ActivityResourceColor(GetActivityPanelColor(gameObject.transform.parent.transform.parent.transform.parent.GetComponent<ActivityInitializer>())); break;

            case MenuObjectType.HappinessBackground: color = gameManager.menuTheme.happinessBackgroundColor; break;
            case MenuObjectType.Happiness: color = gameManager.menuTheme.happinessColor; break;
            case MenuObjectType.HappinessChange: color = gameManager.menuTheme.happinessChangeColor; break;
            case MenuObjectType.HappinessOverflow: color = gameManager.menuTheme.happinessOverflowColor; break;
            case MenuObjectType.HappinessOverflowChange: color = gameManager.menuTheme.happinessOverflowChangeColor; break;

            case MenuObjectType.MoneyBackground: color = gameManager.menuTheme.moneyBackgroundColor; break;
            case MenuObjectType.Money: color = gameManager.menuTheme.moneyColor; break;
            case MenuObjectType.MoneyChange: color = gameManager.menuTheme.moneyChangeColor; break;
            case MenuObjectType.MoneyOverflow: color = gameManager.menuTheme.moneyOverflowColor; break;
            case MenuObjectType.MoneyOverflowChange: color = gameManager.menuTheme.moneyOverflowChangeColor; break;
            case MenuObjectType.MoneyOverflow2: color = gameManager.menuTheme.moneyOverflow2Color; break;
            case MenuObjectType.MoneyOverflow2Change: color = gameManager.menuTheme.moneyOverflow2ChangeColor; break;

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

    private Color GetActivityPanelColor(ActivityInitializer activityInitializer) {
        if(activityInitializer != null) {
            if(activityInitializer.IsFixed()) {
                return gameManager.menuTheme.fixedActivityColor;
            }
            if(activityInitializer.activity != null) {
                switch(activityInitializer.activity.length) {
                    case int n when n >= 4:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return gameManager.menuTheme.dailyTaskLargeColor;
                            case ActivityType.Weekly: return gameManager.menuTheme.weeklyTaskLargeColor;
                            case ActivityType.Bonus: return gameManager.menuTheme.bonusTaskLargeColor;
                        }
                        break;
                    case int n when n >= 2:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return gameManager.menuTheme.dailyTaskMediumColor;
                            case ActivityType.Weekly: return gameManager.menuTheme.weeklyTaskMediumColor;
                            case ActivityType.Bonus: return gameManager.menuTheme.bonusTaskMediumColor;
                        }
                        break;
                    default:
                        switch(activityInitializer.activity.type) {
                            case ActivityType.Daily: return gameManager.menuTheme.dailyTaskSmallColor;
                            case ActivityType.Weekly: return gameManager.menuTheme.weeklyTaskSmallColor;
                            case ActivityType.Bonus: return gameManager.menuTheme.bonusTaskSmallColor;
                        }
                        break;
                }
            }
            
        } return Color.yellow;
    }

    public static bool GetBrightOrDarkColor(Color backgroundColor, int threshold) {
        float backgroundColorBrightness = (0.2126f * (backgroundColor.r * 255)) + (0.7152f * (backgroundColor.g * 255)) + (0.0722f * (backgroundColor.b * 255));
        //Debug.Log(gameObject.name + "'s brightness is " + backgroundColorBrightness + ", compared to the threshold of " + threshold);
        return backgroundColorBrightness <= threshold; // 'true' means light, 'false' means dark
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
    }
}