using UnityEngine;
using TMPro;

public class MenuObject : MonoBehaviour
{
    [SerializeField] private MenuObjectType type;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    //void Update()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        Color color;
        bool isText = false;
            
        switch(type) {
            case MenuObjectType.Background: color = gameManager.menuTheme.backgroundColor; break;

            case MenuObjectType.GridCell: color = gameManager.menuTheme.gridCellColor; break;
            case MenuObjectType.GridHeaderText: color = gameManager.menuTheme.gridHeaderTextColor; isText = true; break;

            case MenuObjectType.DailyTaskList: color = gameManager.menuTheme.dailyTaskListColor; break;
            case MenuObjectType.DailyTaskText: color = gameManager.menuTheme.dailyTaskTextColor; isText = true; break;

            case MenuObjectType.WeeklyTaskList: color = gameManager.menuTheme.weeklyTaskListColor; break;
            case MenuObjectType.WeeklyTaskText: color = gameManager.menuTheme.weeklyTaskTextColor; isText = true; break;

            case MenuObjectType.BonusTaskList: color = gameManager.menuTheme.bonusTaskListColor; break;
            case MenuObjectType.BonusTaskText: color = gameManager.menuTheme.bonusTaskTextColor; isText = true; break;

            case MenuObjectType.Happiness: color = gameManager.menuTheme.happinessColor; break;
            case MenuObjectType.Money: color = gameManager.menuTheme.moneyColor; break;

            case MenuObjectType.FixedActivity: color = gameManager.menuTheme.fixedActivityColor; break;
            case MenuObjectType.FixedActivityBorder: color = gameManager.menuTheme.fixedActivityBorderColor; break;
            case MenuObjectType.ActivityText: color = gameManager.menuTheme.activityTextColor; isText = true; break;

            default: color = Color.red; break;
        };

        if(isText) {
            gameObject.GetComponent<TextMeshProUGUI>().color = color;
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
