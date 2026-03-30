using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoSingleton<GlobalGameManager>
{
    [SerializeField] private Campaign campaign;
    [SerializeField] private int currentWeek = 0;

    private ThemeList menuThemes;
    [SerializeField] private MenuTheme currentTheme;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnUpdateThemeText;

    [SerializeField] private AudioClip clickSound;
    //[SerializeField] private AudioMixer audioMixer;

    protected override void OnInitialize() {
        campaign = Resources.Load<Campaign>("Campaigns/Planscape");
        menuThemes = Resources.Load<ThemeList>("Themes/ThemeList");
        currentTheme = menuThemes.themes[0];

        clickSound = Resources.Load<AudioClip>("Sounds/clickSound");

        //clickSound = 

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
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/clickSound"), Camera.main.transform.position, 1.0f + 0);
    }

    public void CycleTheme() {
        int currentThemeIndex = Instance.menuThemes.themes.IndexOf(Instance.currentTheme);

        if(currentThemeIndex == Instance.menuThemes.themes.Count - 1) { currentThemeIndex = 0; }
        else { currentThemeIndex++; }

        Instance.currentTheme = Instance.menuThemes.themes[currentThemeIndex];
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

    public void Continue() {
        currentWeek = 0;
        SceneManager.LoadScene(1);
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
    }
    
    public void ExitToMenu() {
        SceneManager.LoadScene(0);
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