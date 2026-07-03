using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SimpleMenuObject;

public class SimpleMenuObject : MonoBehaviour, ReceivesThemeUpdates
{
    [SerializeField] private MenuObjectType type;
    [SerializeField] private ThemeTarget target;

    [HideInInspector] public Color color = Color.red;

    void OnEnable() { GlobalGameManager.OnUpdateTheme += OnThemeUpdate; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= OnThemeUpdate; }

    public void OnThemeUpdate() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();

        switch(type) {
            case MenuObjectType.GridCell: {
                color = menuTheme.gridCellColor;
                GridCell gridCell = gameObject.GetComponent<GridCell>();
                if(gridCell != null && gridCell.isFixed) { color = menuTheme.fixedGridCellColor; }
                break;
            }

            case MenuObjectType.ActivityPanel: color = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme); break;
            case MenuObjectType.ActivityShadowPanel: {
                Color temp = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme);
                temp.a = 0.7f;
                color = temp;
                break;
            }

            case MenuObjectType.ActivityResource: color = ActivityResourceColor(GetActivityPanelColor(gameObject.transform.parent.transform.parent.transform.parent.GetComponent<ActivityInitializer>(), menuTheme)); break;
            case MenuObjectType.FixedActivityBorder: color = menuTheme.fixedActivityBorderColor; break;
            case MenuObjectType.TimeHand: {
                TimeHand timeHand = gameObject.GetComponent<TimeHand>();
                if(timeHand != null && timeHand.IsFast()) { color = menuTheme.timeHandFastColor; } 
                else { color = menuTheme.timeHandColor;}
                break;
            }

            case MenuObjectType.BrightText: color = menuTheme.brightTextColor; break;
            case MenuObjectType.DarkText: color = menuTheme.darkTextColor; break;

            case MenuObjectType.PauseButton: color = menuTheme.pauseButtonColor; break;
            case MenuObjectType.HelpButton: color = menuTheme.helpButtonColor; break;

            case MenuObjectType.MenuBackground: color = menuTheme.menuButtonBackgroundColor; break;

            default: break;
        };

        switch(target) {
            case ThemeTarget.SpriteRenderer: gameObject.GetComponent<SpriteRenderer>().color = color; break;
            case ThemeTarget.Image: gameObject.GetComponent<Image>().color = color; break;
            case ThemeTarget.TextMeshPro: gameObject.GetComponent<TextMeshProUGUI>().color = color; break;
            default : break;
        }
    }

    public Color GetMainColor () {
        return color;
    }

    private Color ActivityResourceColor(Color color) {
        float change = GetBrightOrDarkColor(color, 200)? 0.4f : -0.4f;
        float H; float S; float V;
        Color.RGBToHSV(color, out H, out S, out V);

        return Color.HSVToRGB(H, S, V + change);
    }

    private Color GetActivityPanelColor(ActivityInitializer activityInitializer, MenuTheme menuTheme) {
        if(activityInitializer != null) {
            if(activityInitializer.IsFixed()) { return menuTheme.fixedActivityColor; }
            if(activityInitializer.activity != null) {
                int taskListIndex = 0;
                switch(activityInitializer.activityType) {
                    case Activity.Type.Required: taskListIndex = 0; break;
                    case Activity.Type.Bonus: taskListIndex = 2; break;
                }
                return menuTheme.taskListColors[taskListIndex].taskColors[Mathf.Min(activityInitializer.activity.length - 1, menuTheme.taskListColors[taskListIndex].taskColors.Length)];
            }
        }
        Debug.Log("Attempted SimpleMenuObject call of null activity initializer, defaulting to yellow.");
        return Color.yellow;
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
        GridCell,

        ActivityPanel,
        ActivityShadowPanel,
        ActivityResource,
        FixedActivityBorder,

        TimeHand,

        BrightText,
        DarkText,

        PauseButton,
        HelpButton,

        MenuBackground
    }
}