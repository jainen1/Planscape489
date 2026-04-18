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

    [SerializeField] private Scene activeMenuScene;

    protected override void OnInitialize() {
        //campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        themeList = Resources.Load<ThemeList>("Themes/ThemeList");

        Instance.currentTheme = themeList.themes[0];
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

    public void AdvanceWeek() {
        Instance.currentWeek++;
    }

    public void PlayClickSound() {
        //float volume;
        //audioMixer.GetFloat("SFX Volume", out volume);
        AudioSource.PlayClipAtPoint(Instance.clickSound, Camera.main.transform.position, 1.0f + 0);
    }

    public void CycleTheme() {
        Debug.Log("cycling theme from: " + Instance.currentTheme.name);

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

// for testing purposes, allows you to set the theme directly 
    public void SetThemeManual(MenuTheme newTheme)
    {
        Instance.currentTheme = newTheme;
        Instance.SendThemeUpdate(); 
    }
//

    public MenuTheme GetMenuTheme() {
        return Instance.currentTheme;
    }

    public void Options() {
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    public void ThemesMenuScene() {
        SceneManager.LoadScene("ThemeMenu", LoadSceneMode.Additive);
    }

    public void Credits() {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Additive);
    }

    public void Themes() {
        GlobalGameManager.Instance.CycleTheme();
    }

    public void SetCampaignAndPlay() {
        Instance.campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        Instance.currentWeek = 0;
        StartWeek();
    }

    public void StartWeek() {
        if(Instance.campaign != null) {
            SceneManager.LoadScene("LevelScene");
        } else {
            NewGame();
        }
    }

    public void NewGame() {
        SceneManager.LoadScene("CampaignSelectScene", LoadSceneMode.Additive);
    }
    
    public void ExitToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenTutorialScene() {
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);
    }

    public void OpenWinScene() {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Additive);
    }

    public void OpenLoseScene() {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Additive);
    }

    /*public void CloseScene() {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }*/

    public void CloseOptionsScene() {
        SceneManager.UnloadSceneAsync("OptionsMenu");
    }

    public void CloseThemesScene() {
        SceneManager.UnloadSceneAsync("ThemesMenu");
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