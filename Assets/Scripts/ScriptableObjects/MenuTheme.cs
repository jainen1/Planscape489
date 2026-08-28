using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(fileName = "MenuTheme", menuName = "Scriptable Objects/MenuTheme")] [Serializable]
public class MenuTheme : StuffedScriptableObject
{
    [Header("Fonts")]
    //public TMPro.TMP_FontAsset mainFont;
    public FontEnum mainFont;
    public float mainFontSizeScale = 1;
    public float mainCharacterSpacingScale = 1;
    public float mainLineSpacingScale = 1;

    public FontEnum timerFont;
    public float timerFontSizeScale = 1;
    public float timerCharacterSpacingScale = 1;
    public float timerLineSpacingScale = 1;

    public enum FontEnum { Poppins, Digital7, Asimovian, WindowsTahoma }

    [Header("Backgrounds")]
    public BackgroundLayer[] menuBackgroundLayers;
    public BackgroundLayer[] levelBackgroundLayers;
    [Serializable] public class BackgroundLayer {
        public Sprite sprite;
        public Color color;
        public Vector3 position = Vector3.zero;
        public Quaternion rotation;
        //public Vector3 scale = Vector3.one;
        public Vector2 dimensions = new Vector2(19.2f, 10.8f);
    }

    [Header("Task Lists (0 = Required, 2 = Bonus, 1 = Unused)")]
    public TaskListColors[] taskListColors = new TaskListColors[2];
    [Serializable] public class TaskListColors {
        public Color mainColor;
        public Color itemColor;
        public Color scrollbarColor;
        public Color countColor;
        public Color[] taskColors = new Color[4];
    }

    [Header("Resource Bars (0 = Week, 1 = Happiness, 2 = Money)")]
    public ResourceBarColors.Collection[] resourceBarColors = new ResourceBarColors.Collection[3];
    public Color resourceBarBackgroundColor;

    [Serializable] public class ResourceBarColors {
        public Color fill = Color.green;
        public Color change = Color.white;

        [Serializable] public class Collection { public ResourceBarColors[] resourceBars = new ResourceBarColors[0]; }
    }

    [Header("Text Colors")]
    public Color brightTextColor;
    public Color darkTextColor;
    public Color eventTextColor;
    public Color teamPlanscapeColor;

    [Header("Other Colors")]
    public Color menuButtonBackgroundColor;
    public Color pauseButtonColor;
    public Color helpButtonColor;

    public Color gridCellColor;
    public Color fixedGridCellColor;

    public Color fixedActivityColor;
    public Color fixedActivityBorderColor;

    public Color timeHandColor;
    public Color timeHandFastColor;

    [Header("Music")]
    public AudioClip calmMusic;
    public AudioClip tenseMusic;
    public AudioClip superTenseMusic;

    [Header("SFX")]
    public AudioClip buttonClick;

    public AudioClip activityPickUp;
    public AudioClip activityPickUpFail;
    public AudioClip activityPutDown;
    public AudioClip activityTrash;

    public AudioClip[] clockTicking;

    public AudioClip win;
    public AudioClip lose;
    public AudioClip victory;

    public class JData {
        public string version = "v1";
        public string translationKey;
        public int mainFont;
        public float mainFontSizeScale;
        public float mainCharacterSpacingScale;
        public float mainLineSpacingScale;

        public int timerFont;
        public float timerFontSizeScale;
        public float timerCharacterSpacingScale;
        public float timerLineSpacingScale;

        public BackgroundLayerJData[] menuBackgroundLayers;
        public BackgroundLayerJData[] levelBackgroundLayers;

        [Serializable] public class BackgroundLayerJData {
            public string sprite;
            public Color32 color;
            public Vector3 position;
            public Quaternion rotation;
            public Vector2 dimensions;
        }

        public TaskListColorsJData[] taskListColors;

        [Serializable] public class TaskListColorsJData {
            public Color32 mainColor;
            public Color32 itemColor;
            public Color32 scrollbarColor;
            public Color32 countColor;
            public Color32[] taskColors;
        }

        public ResourceBarColorsJData.Collection[] resourceBarColors;
        public Color32 resourceBarBackgroundColor;

        [Serializable] public class ResourceBarColorsJData {
            public Color32 fill;
            public Color32 change;

            [Serializable] public class Collection { public ResourceBarColorsJData[] resourceBars; }
        }

        public Color32 brightTextColor;
        public Color32 darkTextColor;
        public Color32 eventTextColor;
        public Color32 teamPlanscapeColor;

        public Color32 menuButtonBackgroundColor;
        public Color32 pauseButtonColor;
        public Color32 helpButtonColor;

        public Color32 gridCellColor;
        public Color32 fixedGridCellColor;

        public Color32 fixedActivityColor;
        public Color32 fixedActivityBorderColor;

        public Color32 timeHandColor;
        public Color32 timeHandFastColor;

        /*public AudioClip calmMusic;
        public AudioClip tenseMusic;
        public AudioClip superTenseMusic;

        public AudioClip buttonClick;

        public AudioClip activityPickUp;
        public AudioClip activityPickUpFail;
        public AudioClip activityPutDown;
        public AudioClip activityTrash;

        public AudioClip[] clockTicking;

        public AudioClip win;
        public AudioClip lose;
        public AudioClip victory;*/

        public MenuTheme LoadData () {
            MenuTheme menuTheme = ScriptableObject.CreateInstance<MenuTheme>();
            menuTheme.name = translationKey;
            menuTheme.translationKey = translationKey;
            menuTheme.mainFont = (FontEnum) mainFont;
            menuTheme.mainFontSizeScale = mainFontSizeScale;
            menuTheme.mainCharacterSpacingScale = mainCharacterSpacingScale;
            menuTheme.mainLineSpacingScale = mainLineSpacingScale;

            menuTheme.timerFont = (FontEnum) timerFont;
            menuTheme.timerFontSizeScale = timerFontSizeScale;
            menuTheme.timerCharacterSpacingScale = timerCharacterSpacingScale;
            menuTheme.timerLineSpacingScale = timerLineSpacingScale;

            menuTheme.menuBackgroundLayers = new BackgroundLayer[menuBackgroundLayers.Length];
            if(menuBackgroundLayers.Length > 0) { for(int i = 0; i < menuBackgroundLayers.Length; i++) {
                BackgroundLayer backgroundLayer = new BackgroundLayer();
                if(menuBackgroundLayers[i].sprite != null) {
                    backgroundLayer.sprite = JExtraUtility.LoadNewSprite(Path.Combine(Application.streamingAssetsPath, "ContentPacks", menuBackgroundLayers[i].sprite + ".png"));
                }
                backgroundLayer.color = menuBackgroundLayers[i].color;
                backgroundLayer.position = menuBackgroundLayers[i].position;
                backgroundLayer.rotation = menuBackgroundLayers[i].rotation;
                backgroundLayer.dimensions = menuBackgroundLayers[i].dimensions;
                menuTheme.menuBackgroundLayers[i] = backgroundLayer;
            }}

            menuTheme.levelBackgroundLayers = new BackgroundLayer[levelBackgroundLayers.Length];
            if(levelBackgroundLayers.Length > 0) { for(int i = 0; i < levelBackgroundLayers.Length; i++) {
                BackgroundLayer backgroundLayer = new BackgroundLayer();
                if(levelBackgroundLayers[i].sprite != null) {
                    backgroundLayer.sprite = JExtraUtility.LoadNewSprite(Path.Combine(Application.streamingAssetsPath, "ContentPacks", levelBackgroundLayers[i].sprite + ".png"));
                }
                backgroundLayer.color = levelBackgroundLayers[i].color;
                backgroundLayer.position = levelBackgroundLayers[i].position;
                backgroundLayer.rotation = levelBackgroundLayers[i].rotation;
                backgroundLayer.dimensions = levelBackgroundLayers[i].dimensions;
                menuTheme.levelBackgroundLayers[i] = backgroundLayer;
            }}

            menuTheme.taskListColors = new TaskListColors[taskListColors.Length];
            if(taskListColors.Length > 0) { for(int i = 0; i < taskListColors.Length; i++) {
                TaskListColors taskListColorSet = new TaskListColors();
                taskListColorSet.mainColor = taskListColors[i].mainColor;
                taskListColorSet.itemColor = taskListColors[i].itemColor;
                taskListColorSet.scrollbarColor = taskListColors[i].scrollbarColor;
                taskListColorSet.countColor = taskListColors[i].countColor;
                taskListColorSet.taskColors = new Color[taskListColors[i].taskColors.Length];
                if(taskListColors[i].taskColors.Length > 0) { for(int j = 0; j < taskListColors[i].taskColors.Length; j++) {
                    taskListColorSet.taskColors[j] = taskListColors[i].taskColors[j];
                }}
                menuTheme.taskListColors[i] = taskListColorSet;
            }}

            menuTheme.resourceBarColors = new ResourceBarColors.Collection[resourceBarColors.Length];
            if(resourceBarColors.Length > 0) { for(int i = 0; i < resourceBarColors.Length; i++) {
                ResourceBarColors.Collection resourceBarColorCollection = new ResourceBarColors.Collection();
                resourceBarColorCollection.resourceBars = new ResourceBarColors[resourceBarColors[i].resourceBars.Length];
                if(resourceBarColors[i].resourceBars.Length > 0) { for(int j = 0; j < resourceBarColors[i].resourceBars.Length; j++) {
                    ResourceBarColors resourceBarColorSet = new ResourceBarColors();
                    resourceBarColorSet.fill = resourceBarColors[i].resourceBars[j].fill;
                    resourceBarColorSet.change = resourceBarColors[i].resourceBars[j].change;
                    resourceBarColorCollection.resourceBars[j] = resourceBarColorSet;
                }}
                menuTheme.resourceBarColors[i] = resourceBarColorCollection;
            }}
            menuTheme.resourceBarBackgroundColor = resourceBarBackgroundColor;

            menuTheme.brightTextColor = brightTextColor;
            menuTheme.darkTextColor = darkTextColor;
            menuTheme.eventTextColor = eventTextColor;
            menuTheme.teamPlanscapeColor = teamPlanscapeColor;

            menuTheme.menuButtonBackgroundColor = menuButtonBackgroundColor;
            menuTheme.pauseButtonColor = pauseButtonColor;
            menuTheme.helpButtonColor = helpButtonColor;

            menuTheme.gridCellColor = gridCellColor;
            menuTheme.fixedGridCellColor = fixedGridCellColor;

            menuTheme.fixedActivityColor = fixedActivityColor;
            menuTheme.fixedActivityBorderColor = fixedActivityBorderColor;

            menuTheme.timeHandColor = timeHandColor;
            menuTheme.timeHandFastColor = timeHandFastColor;

            /*menuTheme.calmMusic = calmMusic;
            menuTheme.tenseMusic = tenseMusic;
            menuTheme.superTenseMusic = superTenseMusic;

            menuTheme.buttonClick = buttonClick;

            menuTheme.activityPickUp = activityPickUp;
            menuTheme.activityPickUpFail = activityPickUpFail;
            menuTheme.activityPutDown = activityPutDown;
            menuTheme.activityTrash = activityTrash;

            menuTheme.clockTicking = new AudioClip[clockTicking.Length];
            if(clockTicking.Length > 0) { for(int i = 0; i < clockTicking.Length; i++) {
                menuTheme.clockTicking[i] = clockTicking[i];
            }}

            menuTheme.win = win;
            menuTheme.lose = lose;
            menuTheme.victory = victory;*/

            return menuTheme;
        }
    }

    public JData GetAsJData () {
        JData savedState = new JData();
        savedState.translationKey = translationKey;
        savedState.mainFont = (int) mainFont;
        savedState.mainFontSizeScale = mainFontSizeScale;
        savedState.mainCharacterSpacingScale = mainCharacterSpacingScale;
        savedState.mainLineSpacingScale = mainLineSpacingScale;

        savedState.timerFont = (int) timerFont;
        savedState.timerFontSizeScale = timerFontSizeScale;
        savedState.timerCharacterSpacingScale = timerCharacterSpacingScale;
        savedState.timerLineSpacingScale = timerLineSpacingScale;

        savedState.menuBackgroundLayers = new JData.BackgroundLayerJData[menuBackgroundLayers.Length];
        if(menuBackgroundLayers.Length > 0) { for(int i = 0; i < menuBackgroundLayers.Length; i++) {
            JData.BackgroundLayerJData backgroundLayer = new JData.BackgroundLayerJData();
            if(menuBackgroundLayers[i].sprite != null) {
                backgroundLayer.sprite = Path.Combine("PlanscapeGenerated", "Images", menuBackgroundLayers[i].sprite.texture.name); //starts in StreamingAssets/ContentPacks
                JExtraUtility.SaveSprite(menuBackgroundLayers[i].sprite);
            }
            backgroundLayer.color = menuBackgroundLayers[i].color;
            backgroundLayer.position = menuBackgroundLayers[i].position;
            backgroundLayer.rotation = menuBackgroundLayers[i].rotation;
            backgroundLayer.dimensions = menuBackgroundLayers[i].dimensions;
            savedState.menuBackgroundLayers[i] = backgroundLayer;
        }}

        savedState.levelBackgroundLayers = new JData.BackgroundLayerJData[levelBackgroundLayers.Length];
        if(levelBackgroundLayers.Length > 0) { for(int i = 0; i < levelBackgroundLayers.Length; i++) {
            JData.BackgroundLayerJData backgroundLayer = new JData.BackgroundLayerJData();
            if(levelBackgroundLayers[i].sprite != null) {
                backgroundLayer.sprite = Path.Combine("PlanscapeGenerated", "Images", levelBackgroundLayers[i].sprite.texture.name); //starts in StreamingAssets/ContentPacks
                JExtraUtility.SaveSprite(levelBackgroundLayers[i].sprite);
            }
            backgroundLayer.color = levelBackgroundLayers[i].color;
            backgroundLayer.position = levelBackgroundLayers[i].position;
            backgroundLayer.rotation = levelBackgroundLayers[i].rotation;
            backgroundLayer.dimensions = levelBackgroundLayers[i].dimensions;
            savedState.levelBackgroundLayers[i] = backgroundLayer;
        }}

        savedState.taskListColors = new JData.TaskListColorsJData[taskListColors.Length];
        if(taskListColors.Length > 0) { for(int i = 0; i < taskListColors.Length; i++) {
            JData.TaskListColorsJData taskListColorSet = new JData.TaskListColorsJData();
            taskListColorSet.mainColor = taskListColors[i].mainColor;
            taskListColorSet.itemColor = taskListColors[i].itemColor;
            taskListColorSet.scrollbarColor = taskListColors[i].scrollbarColor;
            taskListColorSet.countColor = taskListColors[i].countColor;
            taskListColorSet.taskColors = new Color32[taskListColors[i].taskColors.Length];
            if(taskListColors[i].taskColors.Length > 0) { for(int j = 0; j < taskListColors[i].taskColors.Length; j++) {
                taskListColorSet.taskColors[j] = taskListColors[i].taskColors[j];
            }}
            savedState.taskListColors[i] = taskListColorSet;
        }}

        savedState.resourceBarColors = new JData.ResourceBarColorsJData.Collection[resourceBarColors.Length];
        if(resourceBarColors.Length > 0) { for(int i = 0; i < resourceBarColors.Length; i++) {
            JData.ResourceBarColorsJData.Collection resourceBarColorCollection = new JData.ResourceBarColorsJData.Collection();
            resourceBarColorCollection.resourceBars = new JData.ResourceBarColorsJData[resourceBarColors[i].resourceBars.Length];
            if(resourceBarColors[i].resourceBars.Length > 0) { for(int j = 0; j < resourceBarColors[i].resourceBars.Length; j++) {
                JData.ResourceBarColorsJData resourceBarColorSet = new JData.ResourceBarColorsJData();
                resourceBarColorSet.fill = resourceBarColors[i].resourceBars[j].fill;
                resourceBarColorSet.change = resourceBarColors[i].resourceBars[j].change;
                resourceBarColorCollection.resourceBars[j] = resourceBarColorSet;
            }}
            savedState.resourceBarColors[i] = resourceBarColorCollection;
        }}
        savedState.resourceBarBackgroundColor = resourceBarBackgroundColor;

        savedState.brightTextColor = brightTextColor;
        savedState.darkTextColor = darkTextColor;
        savedState.eventTextColor = eventTextColor;
        savedState.teamPlanscapeColor = teamPlanscapeColor;

        savedState.menuButtonBackgroundColor = menuButtonBackgroundColor;
        savedState.pauseButtonColor = pauseButtonColor;
        savedState.helpButtonColor = helpButtonColor;

        savedState.gridCellColor = gridCellColor;
        savedState.fixedGridCellColor = fixedGridCellColor;

        savedState.fixedActivityColor = fixedActivityColor;
        savedState.fixedActivityBorderColor = fixedActivityBorderColor;

        savedState.timeHandColor = timeHandColor;
        savedState.timeHandFastColor = timeHandFastColor;

        /*savedState.calmMusic = calmMusic;
        savedState.tenseMusic = tenseMusic;
        savedState.superTenseMusic = superTenseMusic;

        savedState.buttonClick = buttonClick;

        savedState.activityPickUp = activityPickUp;
        savedState.activityPickUpFail = activityPickUpFail;
        savedState.activityPutDown = activityPutDown;
        savedState.activityTrash = activityTrash;

        savedState.clockTicking = new AudioClip[clockTicking.Length];
        if(clockTicking.Length > 0) { for(int i = 0; i < clockTicking.Length; i++) {
            savedState.clockTicking[i] = clockTicking[i];
        }}

        savedState.win = win;
        savedState.lose = lose;
        savedState.victory = victory;*/

        return savedState;
    }

    public override void Save () {
        string folderPath = Path.Combine(JExtraUtility.planscapeGeneratedFolder, "Themes");
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        File.WriteAllText(Path.Combine(folderPath, name + ".theme.json"), JsonUtility.ToJson(GetAsJData(), true));
    }
}