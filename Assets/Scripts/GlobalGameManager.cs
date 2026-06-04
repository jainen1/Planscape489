using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField] private Campaign campaign;
    [SerializeField] private int currentWeek = 0;

    [SerializeField] private MenuTheme currentTheme;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnUpdateThemeText;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Scene activeMenuScene;

    [SerializeField] private MenuTheme[] activeThemes; //temporary

    protected override void OnInitialize() {
        SaveAllThemesToJson();
        LoadActiveThemes();

        Instance.clickSound = Resources.Load<AudioClip>("Sounds/clickSound");
        audioMixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");

        Instance.SendThemeUpdate();
        PrintThemes();
    }

    public void PlayClickSound() {
        float volume;
        audioMixer.GetFloat("SFX Volume", out volume);
        AudioSource.PlayClipAtPoint(Instance.clickSound, Camera.main.transform.position, 1.0f * volume);
    }

    // Weeks //

    public Week GetCurrentWeek() { return Instance.campaign.weeks[currentWeek]; }
    public int GetCurrentWeekIndex() { return currentWeek; }
    public int GetLastWeekIndex() { return campaign.weeks.Length; }
    public void AdvanceWeek() { Instance.currentWeek++; }

    // Themes //

    public void CycleTheme() {
        Debug.Log("cycling theme from: " + Instance.currentTheme.name);

        int currentThemeIndex = Array.IndexOf(Instance.activeThemes, Instance.currentTheme);

        if(currentThemeIndex == Instance.activeThemes.Length - 1) { currentThemeIndex = 0; }
        else { currentThemeIndex++; }

        Instance.currentTheme = Instance.activeThemes[currentThemeIndex];
        Instance.SendThemeUpdate();
    }

    public void SendThemeUpdate() {
        OnUpdateTheme();
        OnUpdateThemeText();
    }

    public void SetThemeManually(MenuTheme newTheme) {
        Instance.currentTheme = newTheme;
        Instance.SendThemeUpdate(); 
    }

    public void SetThemeByIndex(int i) {
        Instance.currentTheme = activeThemes[i];
        Instance.SendThemeUpdate();
    }

    public MenuTheme GetCurrentMenuTheme() { return Instance.currentTheme; }
    public MenuTheme[] GetActiveMenuThemes() { return activeThemes; }

    public void SaveAllThemesToJson() {
        MenuTheme[] resourcesThemes = Resources.LoadAll<MenuTheme>("Themes");
        string themesFolder = Path.Combine(Application.streamingAssetsPath, "ContentPacks", "PlanscapeGenerated", "Themes");
        Debug.Log("Writing theme data to " + themesFolder);
        foreach(MenuTheme menuTheme in resourcesThemes) {
            if(!Directory.Exists(themesFolder)) { Directory.CreateDirectory(themesFolder); }
            SaveThemeToJson(themesFolder, menuTheme);
        }
    }

    public void SaveThemeToJson(string path, MenuTheme menuTheme) {

        File.WriteAllText(Path.Combine(path, menuTheme.name.ToLower() + ".theme.json"), JsonUtility.ToJson(menuTheme, true));
    }

    public void LoadActiveThemes() {
        //AssetDatabase.Refresh();
        int themeFileIndex = 0;
        foreach(var file in Directory.EnumerateFiles(Path.Combine(Application.streamingAssetsPath, "ContentPacks", "PlanscapeGenerated", "Themes"), "theme.json")) {
            string json = File.ReadAllText(file);
            JsonUtility.FromJsonOverwrite(json, Instance.activeThemes[themeFileIndex]);
            Debug.Log("Read theme data from file " + file);
        }

        /*if(Instance.GetCurrentMenuTheme() == Instance.GetActiveMenuThemes()[0]) { }*/

        Instance.currentTheme = activeThemes[0];
        Debug.Log("Theme initialized to " + Instance.currentTheme);
    }

    public void PrintThemes() {
        MenuTheme[] themes = Instance.GetActiveMenuThemes();
        string printmessage = "Printing currently loaded themes: ";
        if(themes.Length > 0) {
            for(int i = 0; i < themes.Length; i++) { printmessage += "Theme No. " + i + " \"" + themes[i].name + "\"; "; }
        } else { printmessage += "No themes are loaded."; }
        Debug.Log(printmessage);
    }

    // Scene Management //

    public void SetCampaignAndPlay(Campaign campaign) {
        //Instance.campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        Instance.campaign = campaign;
        Instance.currentWeek = 0;
        Instance.StartWeekWithTutorial();
    }

    public void StartWeekWithTutorial() {
        if(Instance.campaign != null) { MoveToScene("LevelScene"); AddScene("Tutorial"); }
        else { AddScene("CampaignSelect"); }
    }

    public void StartWeek() {
        if(Instance.campaign != null) { MoveToScene("LevelScene"); }
        else { AddScene("CampaignSelect"); }
    }
    
    public void MoveToScene(string scene) { SceneManager.LoadScene(scene); }
    public void AddScene(string scene) { SceneManager.LoadScene(scene, LoadSceneMode.Additive); }
    public void CloseScene(string scene) { SceneManager.UnloadSceneAsync(scene); }
    public void ExitGame() { Application.Quit(); }

    public void OpenPauseScreenIfInLevel() {
        if(FindAnyObjectByType<LevelManager>()) { AddScene("PauseMenu"); }
    }

    public void PauseLevel() { FindFirstObjectByType<LevelManager>().levelIsActive = false; }
    public void UnPauseLevel() { FindFirstObjectByType<LevelManager>().levelIsActive = true; }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if(scene.name == "LevelScene") {
            LevelManager levelManager = FindFirstObjectByType<LevelManager>();
            if(levelManager != null) { levelManager.StartLevel(); }
        }
    }
}