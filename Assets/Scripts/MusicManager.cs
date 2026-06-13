using UnityEngine;

public class MusicManager : MonoSingleton<MusicManager>
{
    [SerializeField] private AudioSource audioSource;

    void OnEnable() { GlobalGameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= UpdateMenuObject; }

    public void UpdateMenuObject() {
        AudioClip themeMusic = GlobalGameManager.GetCurrentMenuTheme().music;
        if(themeMusic != null && themeMusic != audioSource.clip) {
            audioSource.clip = themeMusic;
            audioSource.Play();
        }
    }
}