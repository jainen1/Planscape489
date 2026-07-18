using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private int activeContentIndex;

    public void Awake() {
        LevelManager.Instance.pauseMenuInteractible = false;
        LevelManager.Instance.levelIsActive = false;
        GlobalGameManager.SendThemeUpdate();
        activeContentIndex = 0;
        content.text = GlobalGameManager.GetCurrentWeek().tutorialContent[activeContentIndex];
    }

    public void AdvanceTutorialOrEnd() {
        activeContentIndex++;
        if(activeContentIndex >= GlobalGameManager.GetCurrentWeek().tutorialContent.Length) { //end tutorial
            LevelManager.Instance.pauseMenuInteractible = true;
            LevelManager.Instance.levelIsActive = true;
            GlobalGameManager.CloseScene("Tutorial");
        } else {
            content.text = GlobalGameManager.GetCurrentWeek().tutorialContent[activeContentIndex];
        }
    }
}