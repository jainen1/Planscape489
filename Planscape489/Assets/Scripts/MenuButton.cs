using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MenuButton : MonoBehaviour
{
    private LevelManager gameManager;

    private MenuTheme[] menuThemes;
    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<LevelManager>();
        menuThemes = Resources.LoadAll<MenuTheme>("Themes");
        Themes();
    }

    public void PlayClickSound() {
        gameManager.PlayClickSound();
    }

    public void SkipTimer() {
        FindFirstObjectByType<TimeHandSprite>().timer = 0;
    }

    public void Resume() {
        gameManager.TogglePause();
    }

    public void RestartWeek() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Options() {

    }

    public void Themes() {
        if(index == menuThemes.Length - 1) { index = 0; }
        else { index++; }
        gameManager.menuTheme = menuThemes[index];
        gameManager.SendThemeUpdate();
    }

    public void Exit() {
        Application.Quit();
        /*if(Application.isEditor) {
            UnityEditor.EditorApplication.isPlaying = false;
        }*/
    }
}