using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}