using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMenuObject : MonoBehaviour, ReceivesThemeUpdates
{
    [SerializeField] private MenuObjectType type;
    [SerializeField] private ThemeTarget target;
    private enum ThemeTarget { SpriteRenderer, Image, TextMeshPro }

    [SerializeField] private Color color = Color.red;

    void OnEnable() { GlobalGameManager.OnUpdateTheme += OnThemeUpdate; LevelManager.OnTimeHandSpeedChange += OnThemeUpdate; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= OnThemeUpdate; LevelManager.OnTimeHandSpeedChange -= OnThemeUpdate; }

    public void OnThemeUpdate() {
        color = GetComplexColorForType(type);

        switch(target) {
            //case ThemeTarget.SpriteRenderer: gameObject.GetComponent<SpriteRenderer>().color = color; break;
            case ThemeTarget.Image: gameObject.GetComponent<Image>().color = color; break;
            case ThemeTarget.TextMeshPro: gameObject.GetComponent<TextMeshProUGUI>().color = color; break;
            default : break;
        }
    }

    public Color GetComplexColorForType (MenuObjectType type) {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();
        switch(type) {
            case MenuObjectType.GridCell: {
                GridCell gridCell = gameObject.GetComponent<GridCell>();
                if(gridCell != null && gridCell.isFixed) { return menuTheme.fixedGridCellColor; }
                return menuTheme.gridCellColor;
            }

            case MenuObjectType.ActivityPanel: return GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme);
            case MenuObjectType.ActivityShadowPanel: {
                Color temp = GetActivityPanelColor(gameObject.transform.parent.GetComponent<ActivityInitializer>(), menuTheme);
                temp.a = 0.7f; return temp;
            }

            case MenuObjectType.ActivityResource: return ActivityResourceColor(GetActivityPanelColor(gameObject.transform.parent.transform.parent.transform.parent.GetComponent<ActivityInitializer>(), menuTheme));
            default: { return GetColorForType(type); }
        };
    }

    public static Color GetColorForType (MenuObjectType type) {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();
        switch(type) {
            case MenuObjectType.GridCell: return menuTheme.gridCellColor;
            case MenuObjectType.FixedGridCell: return menuTheme.fixedGridCellColor;

            case MenuObjectType.FixedActivityBorder: return menuTheme.fixedActivityBorderColor;
            case MenuObjectType.TimeHand: { return Color.Lerp(menuTheme.timeHandColor, menuTheme.timeHandFastColor, LevelManager.Instance.timeHand.fastForward.value); }

            case MenuObjectType.BrightText: return menuTheme.brightTextColor;
            case MenuObjectType.DarkText: return menuTheme.darkTextColor;
            case MenuObjectType.EventText: return menuTheme.eventTextColor;
            case MenuObjectType.SubtitleText: return menuTheme.teamPlanscapeColor;

            case MenuObjectType.PauseButton: return menuTheme.pauseButtonColor;
            case MenuObjectType.HelpButton: return menuTheme.helpButtonColor;

            case MenuObjectType.MenuBackground: return menuTheme.menuButtonBackgroundColor;

            default: return Color.red;
        }
    }

    public Color GetMainColor () { return color; }

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

        Vector3 brightnessCoefficients = new Vector3(0.2126f, 0.7152f, 0.0722f);
        //return Vector3.Dot(brightnessCoefficients, new Vector3(backgroundColor.r, backgroundColor.g, backgroundColor.b)) <= threshold; // 'true' means light, 'false' means dark

        //Debug.Log("This object's brightness is " + backgroundColorBrightness + ", which is " + (brighter? "brighter" : "darker")  + " than the threshold of " + threshold + ".");
        return brighter;
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

        MenuBackground,

        EventText,
        SubtitleText,

        FixedGridCell
    }
}