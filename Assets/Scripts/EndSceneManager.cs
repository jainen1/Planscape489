using System;
using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public GameObject nextWeek;
    public GameObject restartWeek;
    public GameObject exitToMenu;

    public void Awake () {
        SetParameters(FindFirstObjectByType<LevelManager>().activeEndScreen);
    }

    public void SetParameters(EndSceneScreen endSceneScreen) {
        AudioSource.PlayClipAtPoint(endSceneScreen.sound, Camera.main.transform.position, 1.0f);

        title.text = endSceneScreen.title;
        description.text = endSceneScreen.description;
        spriteRenderer.color = endSceneScreen.color;
        nextWeek.SetActive(endSceneScreen.next);
        restartWeek.SetActive(endSceneScreen.restart);
        exitToMenu.SetActive(endSceneScreen.exit);
    }
}

[Serializable]
public class EndSceneScreen {
    public string title;
    public string description;
    public AudioClip sound;
    public Color color;
    public bool next;
    public bool restart;
    public bool exit;
}