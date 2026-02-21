using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private LevelManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<LevelManager>();
    }

    public void PlayClickSound() {
        AudioSource.PlayClipAtPoint(gameManager.clickSound, Camera.main.transform.position);
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

    }

    public void Exit() {
        Application.Quit();
        /*if(Application.isEditor) {
            UnityEditor.EditorApplication.isPlaying = false;
        }*/
    }
}