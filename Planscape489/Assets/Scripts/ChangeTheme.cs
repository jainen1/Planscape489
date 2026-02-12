using UnityEngine;

public class ChangeTheme : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] MenuTheme[] menuThemes;
    [SerializeField] private AudioClip clickSound;

    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.menuTheme = menuThemes[index];
        gameManager.UpdateTheme();
    }

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);

        if(index == menuThemes.Length-1) { index = 0; }
        else { index++; }
        gameManager.menuTheme = menuThemes[index];
        gameManager.UpdateTheme();
    }
}
