using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject visibleWhileTutorial;
    [SerializeField] List<GameObject> screens;

    private int activeScreenIndex;

    public void InitializeTutorial() {
        activeScreenIndex = 0;
        foreach(GameObject screen in screens) {
            screen.transform.localScale = Vector3.zero;
        }
        screens[activeScreenIndex].transform.localScale = Vector3.one;
    }

    public void AdvanceTutorialOrEnd() {
        activeScreenIndex++;
        screens[activeScreenIndex - 1].transform.localScale = Vector3.zero;
        if(screens.Count < activeScreenIndex) { //end tutorial
            transform.localScale = Vector3.zero;
        } else {
            screens[activeScreenIndex].transform.localScale = Vector3.one;
        }
    }
}