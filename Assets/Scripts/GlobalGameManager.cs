using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField] private Campaign campaign;
    [SerializeField] private int currentWeek = 0;

    [SerializeField] private List<MenuTheme> loadedThemes;
    [SerializeField] private MenuTheme defaultTheme;
    [SerializeField] private int currentThemeIndex;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnUpdateThemeText;

    [SerializeField] private Scene activeMenuScene;

    [SerializeField] public List<TMPro.TMP_FontAsset> activeFonts;
    [SerializeField] public List<LanguageFile> activeLangs;

    public GameSettings settings;

    protected override void OnInitialize() {
        Instance.currentThemeIndex = 0;
        LoadThemes();

        //SaveGame();
        StartCoroutine(SendLateThemeUpdate());

        foreach(string file in JExtraUtility.LoadJsonFilesOfType("Lang")) {
            Instance.activeLangs.Add(JsonUtility.FromJson<LanguageFile.JData>(File.ReadAllText(file)).LoadData()); }
    }

    System.Collections.IEnumerator SendLateThemeUpdate () {
        yield return null;
        SendThemeUpdate();
    }

    //public void WakeUp () { Debug.Log("GlobalGameManager summoned."); }

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

    public static void LoadThemes () {
        Instance.loadedThemes.Clear();
        try{ 
            foreach(string file in JExtraUtility.LoadJsonFilesOfType("Themes")) {
                Instance.loadedThemes.Add(JsonUtility.FromJson<MenuTheme.JData>(File.ReadAllText(file)).LoadData()); }
        } catch(Exception e) {
            Debug.LogError("MenuTheme deserialization failed.");
            //throw;
        }
        Debug.Log("Theme initialized to " + GetCurrentMenuTheme());
    }

    public static void CycleTheme() {
        Debug.Log("cycling theme from: " + GetCurrentMenuTheme().name);
        if(Instance.currentThemeIndex == Instance.loadedThemes.Count - 1) { Instance.currentThemeIndex = 0; }
        else {Instance.currentThemeIndex++; }
        SendThemeUpdate();
    }

    public static void SendThemeUpdate() {
        OnUpdateTheme();
        OnUpdateThemeText();
    }

    /*public static void SetThemeManually(MenuTheme newTheme) {
        Instance.currentTheme = newTheme;
        SendThemeUpdate();
        //Debug.Log("Switched to: " + GlobalGameManager.Instance.GetActiveMenuThemes()[themeIndex].name);
    }*/

    public static void SetThemeByIndex(int i) {
        Instance.currentThemeIndex = i;
        SendThemeUpdate();
    }

    public static MenuTheme GetCurrentMenuTheme() {
        MenuTheme currentTheme = Instance.loadedThemes[Instance.currentThemeIndex];
        if(currentTheme == null) { return Instance.defaultTheme; }
        return currentTheme;
    }

    public static List<MenuTheme> GetLoadedThemes() { return Instance.loadedThemes; }

    public static void PrintThemes() {
        List<MenuTheme> themes = GetLoadedThemes();
        string printmessage = "Printing currently loaded themes: ";
        if(themes.Count > 0) {
            for(int i = 0; i < themes.Count; i++) { printmessage += "Theme No. " + i + " \"" + themes[i].name + "\"; "; }
        } else { printmessage += "No themes are loaded."; }
        Debug.Log(printmessage);
    }

    // Scene Management //

    public static void SetCampaignAndPlay(Campaign campaign) {
        if(campaign.weeks.Length > 0) {
            //Instance.campaign = Resources.Load<Campaign>("Campaigns/Planscape");
            Instance.campaign = campaign;
            Instance.currentWeek = 0;
            //StartWeekWithTutorial();
            StartWeek();
        } else { Debug.LogWarning("Attempted to play campaign with zero weeks. Don't do that please."); }
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