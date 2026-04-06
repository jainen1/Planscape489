using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField] private Campaign campaign;
    [SerializeField] private int currentWeek = 0;

    private ThemeList themeList;
    [SerializeField] private MenuTheme currentTheme;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnUpdateThemeText;

    [SerializeField] private AudioClip clickSound;
    //[SerializeField] private AudioMixer audioMixer;

    protected override void OnInitialize() {
        //campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        themeList = Resources.Load<ThemeList>("Themes/ThemeList");
        Instance.currentTheme = themeList.themes[0];

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

    public void AdvanceWeek() {
        Instance.currentWeek++;
        Continue();
    }

    public void RestartWeek() {
        SceneManager.LoadScene(1);
    }

    public void PlayClickSound() {
        //float volume;
        //audioMixer.GetFloat("SFX Volume", out volume);
        AudioSource.PlayClipAtPoint(Instance.clickSound, Camera.main.transform.position, 1.0f + 0);
    }

    public void CycleTheme() {
        int currentThemeIndex = Instance.themeList.themes.IndexOf(Instance.currentTheme);

        if(currentThemeIndex == Instance.themeList.themes.Count - 1) { currentThemeIndex = 0; }
        else { currentThemeIndex++; }

        Instance.currentTheme = Instance.themeList.themes[currentThemeIndex];
        Instance.SendThemeUpdate();
    }

    public void SendThemeUpdate() {
        OnUpdateTheme();
        OnUpdateThemeText();
    }

    public MenuTheme GetMenuTheme() {
        return Instance.currentTheme;
    }

    public void Options() {

    }

    public void Credits() {

    }

    public void Themes() {
        GlobalGameManager.Instance.CycleTheme();
    }

    public void SetCampaignAndPlay() {
        campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        currentWeek = 0;
        Continue();
    }

    public void Continue() {
        if(campaign != null) {
            SceneManager.LoadScene("LevelScene");
        } else {
            NewGame();
        }
    }

    public void NewGame() {
        SceneManager.LoadScene("CampaignSelectScene");
    }
    
    public void ExitToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitGame() {
        Application.Quit();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if(levelManager != null) { levelManager.StartLevel(); }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}