using System;
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
    //[SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Scene activeMenuScene;

    [SerializeField] private MenuTheme[] themeList; //temporary

    protected override void OnInitialize() {
        themeList = Resources.LoadAll<MenuTheme>("Themes");

        Instance.currentTheme = themeList[0];
        Debug.Log("theme being initialized to: " + Instance.currentTheme);


        Instance.clickSound = Resources.Load<AudioClip>("Sounds/clickSound");

        //audioMixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");

        Instance.SendThemeUpdate();
    }

    public Week GetCurrentWeek() {
        return Instance.campaign.weeks[currentWeek];
    }

    public int GetCurrentWeekIndex() {
        return currentWeek;
    }

    public int GetLastWeekIndex() {
        return campaign.weeks.Length;
    }

    public void AdvanceWeek() {
        Instance.currentWeek++;
    }

    public MenuTheme[] GetThemeList() {
        return themeList;
    }

    public void PlayClickSound() {
        //float volume;
        //audioMixer.GetFloat("SFX Volume", out volume);
        AudioSource.PlayClipAtPoint(Instance.clickSound, Camera.main.transform.position, 1.0f + 0);
    }

    public void CycleTheme() {
        Debug.Log("cycling theme from: " + Instance.currentTheme.name);

        int currentThemeIndex = Array.IndexOf(Instance.themeList, Instance.currentTheme);

        if(currentThemeIndex == Instance.themeList.Length - 1) { currentThemeIndex = 0; }
        else { currentThemeIndex++; }

        Instance.currentTheme = Instance.themeList[currentThemeIndex];
        Instance.SendThemeUpdate();
    }

    public void SendThemeUpdate() {
        OnUpdateTheme();
        OnUpdateThemeText();
    }

    public void SetThemeManual(MenuTheme newTheme)
    {
        Instance.currentTheme = newTheme;
        Instance.SendThemeUpdate(); 
    }

    public void SetThemeByIndex(int i) {
        Instance.currentTheme = themeList[i];
        Instance.SendThemeUpdate();
    }

    public MenuTheme GetMenuTheme() {
        return Instance.currentTheme;
    }

    public void PauseLevel() {
        FindFirstObjectByType<LevelManager>().levelIsActive = false;
    }

    public void UnPauseLevel() {
        FindFirstObjectByType<LevelManager>().levelIsActive = true;
    }

    public void SetCampaignAndPlay(Campaign campaign) {
        //Instance.campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        Instance.campaign = campaign;
        Instance.currentWeek = 0;
        Instance.StartWeekWithTutorial();
    }

    public void StartWeekWithTutorial() {
        if(Instance.campaign != null) {
            MoveToScene("LevelScene");
            AddScene("Tutorial");
        }
        else {
            AddScene("CampaignSelect");
        }
    }

    public void StartWeek() {
        if(Instance.campaign != null) {
            MoveToScene("LevelScene");
        }
        else {
            AddScene("CampaignSelect");
        }
    }
    
    public void MoveToScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void AddScene(string scene) {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void CloseScene(string scene) {
        SceneManager.UnloadSceneAsync(scene);
    }

    public void OpenPauseScreenIfInLevel() {
        if(FindAnyObjectByType<LevelManager>()) {
            AddScene("PauseMenu");
        }
    }

    public void ExitGame() { Application.Quit(); }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if(scene.name == "LevelScene") {
            LevelManager levelManager = FindFirstObjectByType<LevelManager>();
            if(levelManager != null) { levelManager.StartLevel(); }
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}