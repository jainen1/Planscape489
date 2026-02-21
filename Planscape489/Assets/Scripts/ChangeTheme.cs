using UnityEngine;

public class ChangeTheme : MonoBehaviour
{
    private LevelManager gameManager;

    [SerializeField] MenuTheme[] menuThemes;
    [SerializeField] private AudioClip clickSound;

    private int index = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuThemes = Resources.LoadAll<MenuTheme>("Themes");

        gameManager = FindFirstObjectByType<LevelManager>();
        gameManager.menuTheme = menuThemes[index];
        gameManager.InUpdateTheme();
    }

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1.0f);
        CycleTheme();
    }

    public void CycleTheme() {
        if(index == menuThemes.Length - 1) { index = 0; }
        else { index++; }
        gameManager.menuTheme = menuThemes[index];
        gameManager.InUpdateTheme();
    }
}