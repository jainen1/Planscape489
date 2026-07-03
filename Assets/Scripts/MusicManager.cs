using UnityEngine;

public class MusicManager : MonoSingleton<MusicManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioLowPassFilter lowPassFilter;

    public bool shouldMuffleMusicOnPauseScreen;

    void OnEnable() { GlobalGameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= UpdateMenuObject; }

    public void UpdateMenuObject() {
        AudioClip themeMusic = GlobalGameManager.GetCurrentMenuTheme().calmMusic;
        if(FindAnyObjectByType<LevelManager>()) { //test if you are in the level
            switch(GlobalGameManager.GetCurrentWeek().musicType) {
                case MusicType.SuperTense: { themeMusic = GlobalGameManager.GetCurrentMenuTheme().superTenseMusic; if(themeMusic != null) {  break; } else { goto case MusicType.Tense; } }
                case MusicType.Tense: { themeMusic = GlobalGameManager.GetCurrentMenuTheme().tenseMusic; if(themeMusic != null) {  break; } else { goto case MusicType.Calm; } }
                case MusicType.Calm: { themeMusic = GlobalGameManager.GetCurrentMenuTheme().calmMusic; break; }
            }
        }

        if(themeMusic != null && themeMusic != musicSource.clip) {
            musicSource.clip = themeMusic;
            musicSource.Play();
        }
    }

    public void MuffleMusicIfInLevel(bool muffle) {
        if(FindAnyObjectByType<LevelManager>() && shouldMuffleMusicOnPauseScreen) {
            lowPassFilter.enabled = muffle;
            musicSource.volume = muffle ? 0.7f : 1.0f;
        }
    }
}