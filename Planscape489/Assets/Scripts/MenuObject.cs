using TMPro;
using UnityEngine;

public class MenuObject : MonoBehaviour
{
    [SerializeField] private MenuObjectType type;

    private GameManager gameManager;

    void OnEnable() { GameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GameManager.OnUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<GameManager>();

        Color color = Color.red;
        bool isText = false;

        switch(type) {
            case MenuObjectType.Background: color = gameManager.menuTheme.backgroundColor; break;

            case MenuObjectType.GridCell: color = gameManager.menuTheme.gridCellColor; break;
            //case MenuObjectType.GridHeaderText: color = gameManager.menuTheme.gridHeaderTextColor; isText = true; break;
            case MenuObjectType.GridHeaderText: color = GetBrightOrDarkTextColor(gameManager.menuTheme.gridCellColor, 128) ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor; isText = true; break;

            case MenuObjectType.DailyTaskList: color = gameManager.menuTheme.dailyTaskListColor; break;
            //case MenuObjectType.DailyTaskText: color = gameManager.menuTheme.dailyTaskTextColor; isText = true; break;
            case MenuObjectType.DailyTaskText: color = GetBrightOrDarkTextColor(gameManager.menuTheme.dailyTaskListColor, 128) ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor; isText = true; break;

            case MenuObjectType.WeeklyTaskList: color = gameManager.menuTheme.weeklyTaskListColor; break;
            //case MenuObjectType.WeeklyTaskText: color = gameManager.menuTheme.weeklyTaskTextColor; isText = true; break;
            case MenuObjectType.WeeklyTaskText: color = GetBrightOrDarkTextColor(gameManager.menuTheme.weeklyTaskListColor, 128) ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor; isText = true; break;

            case MenuObjectType.BonusTaskList: color = gameManager.menuTheme.bonusTaskListColor; break;
            //case MenuObjectType.BonusTaskText: color = gameManager.menuTheme.bonusTaskTextColor; isText = true; break;
            case MenuObjectType.BonusTaskText: color = GetBrightOrDarkTextColor(gameManager.menuTheme.bonusTaskListColor, 128) ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor; isText = true; break;

            case MenuObjectType.Happiness: color = gameManager.menuTheme.happinessColor; break;
            case MenuObjectType.Money: color = gameManager.menuTheme.moneyColor; break;

            case MenuObjectType.ActivityPanel: color = GetActivityPanelColor(gameObject.transform.parent.GetComponentInChildren<ActivityInitializer>()); break;
            case MenuObjectType.ActivityShadowPanel: {
                Color temp = GetActivityPanelColor(gameObject.transform.parent.GetComponentInChildren<ActivityInitializer>());
                temp.a = 0.7f;
                color = temp;
                break;
            }

            case MenuObjectType.ActivityText: {
                bool brightOrDark = GetBrightOrDarkTextColor(GetActivityPanelColor(gameObject.transform.parent.transform.parent.GetComponent<ActivityInitializer>()), 230);
                color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
                isText = true;
                break;
            }

            case MenuObjectType.FixedActivityBorder: color = gameManager.menuTheme.fixedActivityBorderColor; break;
            default: break;
        }
        ;

        if(isText) {
            gameObject.GetComponent<TextMeshProUGUI>().color = color;
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private Color GetActivityPanelColor(ActivityInitializer activityInitializer) {
        if(activityInitializer != null) {
            if(activityInitializer.IsFixed()) {
                return gameManager.menuTheme.fixedActivityColor;
            }
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
        } return Color.yellow;
    }

    private bool GetBrightOrDarkTextColor(Color backgroundColor, int threshold) {
        float backgroundColorBrightness = (0.2126f * (backgroundColor.r * 255)) + (0.7152f * (backgroundColor.g * 255)) + (0.0722f * (backgroundColor.b * 255));
        //Debug.Log(gameObject.name + "'s brightness is " + backgroundColorBrightness + ", compared to the threshold of " + threshold);
        return backgroundColorBrightness <= threshold; // 'true' means light, 'false' means dark
    }
}