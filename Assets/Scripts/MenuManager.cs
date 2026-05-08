using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start() {
        GlobalGameManager.Instance.SendThemeUpdate(); //removing this single line of code prevents the theme system AND GlobalGameManager from loading at the start of a game. I have tried to migrate it. I have failed.
    }
}