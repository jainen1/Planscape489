using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsGameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit() {
        Debug.Log("clicked quit");
        Application.Quit(); 

    }

    public void BackToMain() {
        Debug.Log("going back to main");
        SceneManager.LoadScene("MainMenuScene");
    }
}
