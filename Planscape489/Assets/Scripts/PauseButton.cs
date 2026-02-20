using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private AudioClip clickSound;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, Vector3.zero, 1.0f);
        if(gameManager.paused) { gameManager.UnPauseScene(); }
        else { gameManager.PauseScene(); }
    }
}