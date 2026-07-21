using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField] private Campaign campaign;
    [SerializeField] private int currentWeek = 0;

    [SerializeField] private MenuTheme currentTheme;
    [SerializeField] private MenuTheme defaultTheme;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnUpdateThemeText;

    [SerializeField] private Scene activeMenuScene;

    [SerializeField] private MenuTheme[] activeThemes; //temporary

    public GameSettings settings;

    protected override void OnInitialize() {
        SaveAllThemesToJson();
        //LoadActiveThemes();
        LoadThemesFromResources();

        //SaveGame();

        SendThemeUpdate();
    }

    // Weeks //

    public static Week GetCurrentWeek() { return Instance.campaign.weeks[Instance.currentWeek]; }
    public static int GetCurrentWeekIndex() { return Instance.currentWeek; }
    public static int GetLastWeekIndex() { return Instance.campaign.weeks.Length; }
    public static void AdvanceWeek() { Instance.currentWeek++; }

    public static void SaveGame () {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save.json"); //previously "PlanscapeSave "+*/DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss")+".plansave.json"
        GameSave gameData = new GameSave();
        if(Instance.campaign != null) {
            gameData.currentCampaign = Instance.campaign.ToString();
            gameData.week = GetCurrentWeekIndex();
        }

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(gameData, true));
        Debug.Log("Wrote new save data to " + saveFilePath);
    }

    public static void SaveSettings () {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "settings.save.json");
        GameSettings gameSettings = new GameSettings();
        SoundManager.GetAudioMixer().GetFloat(SoundManager.AudioChannels.music + " Volume", out gameSettings.musicVolume);
        SoundManager.GetAudioMixer().GetFloat(SoundManager.AudioChannels.sfx + " Volume", out gameSettings.sfxVolume);

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(gameSettings, true));
        Debug.Log("Wrote new settings save data to " + saveFilePath);
    }

    // Themes //

    public static void CycleTheme() {
        Debug.Log("cycling theme from: " + Instance.currentTheme.name);

        int currentThemeIndex = Array.IndexOf(Instance.activeThemes, Instance.currentTheme);

        if(currentThemeIndex == Instance.activeThemes.Length - 1) { currentThemeIndex = 0; }
        else { currentThemeIndex++; }

        Instance.currentTheme = Instance.activeThemes[currentThemeIndex];
        SendThemeUpdate();
    }

    public static void SendThemeUpdate() {
        OnUpdateTheme();
        OnUpdateThemeText();
    }

    public static void SetThemeManually(MenuTheme newTheme) {
        Instance.currentTheme = newTheme;
        SendThemeUpdate(); 
    }

    public static void SetThemeByIndex(int i) {
        Instance.currentTheme = Instance.activeThemes[i];
        SendThemeUpdate();
    }

    public static MenuTheme GetCurrentMenuTheme() {
        if(Instance.currentTheme == null) {
            return Instance.defaultTheme;
        } return Instance.currentTheme;
    }

    public static MenuTheme[] GetActiveMenuThemes() { return Instance.activeThemes; }

    private static string themesFolder = Path.Combine(Application.streamingAssetsPath, "ContentPacks", "PlanscapeGenerated", "Themes");
    private static string campaignsFolder = Path.Combine(Application.streamingAssetsPath, "ContentPacks", "PlanscapeGenerated", "Campaigns");

    public void SaveAllThemesToJson() {
        MenuTheme[] resourcesThemes = Resources.LoadAll<MenuTheme>("Themes");
        Debug.Log("Writing theme data to " + themesFolder);
        foreach(MenuTheme menuTheme in resourcesThemes) {
            if(!Directory.Exists(themesFolder)) { Directory.CreateDirectory(themesFolder); }
            File.WriteAllText(Path.Combine(themesFolder, menuTheme.name.ToLower() + ".theme.json"), JsonUtility.ToJson(menuTheme, true)); // save theme to JSON
        }
    }

    public static void LoadThemesFromResources () { // temporary theme loader
        Instance.activeThemes = Resources.LoadAll<MenuTheme>("Themes");
        Instance.currentTheme = Instance.activeThemes[0];
        Debug.Log("Theme initialized to " + Instance.currentTheme);
    }

    public static void LoadActiveThemes() {
        //AssetDatabase.Refresh();
        int themeFileIndex = 0;
        foreach(var file in Directory.EnumerateFiles(themesFolder, "theme.json")) {
            string json = File.ReadAllText(file);
            JsonUtility.FromJsonOverwrite(json, Instance.activeThemes[themeFileIndex]);
            Debug.Log("Read theme data from file " + file);
        }

        /*if(Instance.GetCurrentMenuTheme() == Instance.GetActiveMenuThemes()[0]) { }*/

        Instance.currentTheme = Instance.activeThemes[0];
        Debug.Log("Theme initialized to " + Instance.currentTheme);
    }

    public static void PrintThemes() {
        MenuTheme[] themes = GetActiveMenuThemes();
        string printmessage = "Printing currently loaded themes: ";
        if(themes.Length > 0) {
            for(int i = 0; i < themes.Length; i++) { printmessage += "Theme No. " + i + " \"" + themes[i].name + "\"; "; }
        } else { printmessage += "No themes are loaded."; }
        Debug.Log(printmessage);
    }

    // Scene Management //

    public static void SetCampaignAndPlay(Campaign campaign) {
        //Instance.campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        Instance.campaign = campaign;
        Instance.currentWeek = 0;
        //StartWeekWithTutorial();
        StartWeek();
    }

    public static void StartWeekWithTutorial() {
        if(Instance.campaign != null) { MoveToScene("LevelScene"); AddScene("Tutorial"); }
        else { AddScene("CampaignSelect"); }
    }

    public static void StartWeek() {
        if(Instance.campaign != null) { MoveToScene("LevelScene"); }
        else { AddScene("CampaignSelect"); }
    }
    
    public static void MoveToScene(string scene) { SceneManager.LoadScene(scene); }
    public static void AddScene(string scene) { SceneManager.LoadScene(scene, LoadSceneMode.Additive); }
    public static void CloseScene(string scene) { SceneManager.UnloadSceneAsync(scene); }
    public static void ExitGame() { Application.Quit(); }

    public static void OpenPauseScreenIfInLevel() {
        if(FindAnyObjectByType<LevelManager>()) { AddScene("PauseMenu"); }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if(scene.name == "LevelScene") {
            LevelManager levelManager = FindFirstObjectByType<LevelManager>();
            if(levelManager != null) { levelManager.StartLevel(); }
        }
        else if(scene.name == "EndScene") {
            EndSceneManager endSceneManager = FindFirstObjectByType<EndSceneManager>();
            if(endSceneManager != null) { endSceneManager.SetParameters(LevelManager.Instance.activeEndScreen); }
        }
    }
}