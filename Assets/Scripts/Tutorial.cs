using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject visibleWhileTutorial;
    [SerializeField] List<GameObject> screens;

    [SerializeField] private int activeScreenIndex;

    private LevelManager levelManager;

    public void Awake() {
        levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.menuPaused = true;
        activeScreenIndex = 0;
        foreach(GameObject screen in screens) {
            screen.transform.localScale = Vector3.zero;
        }
        screens[activeScreenIndex].transform.localScale = Vector3.one;
    }

    public void AdvanceTutorialOrEnd() {
        activeScreenIndex++;
        screens[activeScreenIndex - 1].transform.localScale = Vector3.zero;
        if(activeScreenIndex >= screens.Count) { //end tutorial
            transform.localScale = Vector3.zero;
            levelManager.menuPaused = false;
            GlobalGameManager.Instance.CloseTutorialScene();
        } else {
            screens[activeScreenIndex].transform.localScale = Vector3.one;
        }
    }
}