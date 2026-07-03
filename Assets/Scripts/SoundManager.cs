using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;

    public static AudioMixer GetAudioMixer() { return Instance.audioMixer; }

    public static void PlayClip(AudioClip clip, [UnityEngine.Internal.DefaultValue("SFX")] string channelName) {
        // from here on is an edited version of the 'AudioSource.PlayClipAtPoint' function. 'AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Pow(10f, volume / 20));'
        GameObject gameObject = new GameObject("TemporaryAudio(" + clip.name + ")");
        gameObject.transform.position = Camera.main.transform.position;
        DontDestroyOnLoad(gameObject);

        AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = Instance.audioMixer.FindMatchingGroups(channelName)[0]; //previously 'audioSource.volume = Mathf.Pow(10f, volume / 20);'
        audioSource.bypassEffects = true;
        audioSource.Play();
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public static void PlayClickSound() {
        if(GlobalGameManager.GetCurrentMenuTheme().buttonClick == null) { return; }
        PlayClip(GlobalGameManager.GetCurrentMenuTheme().buttonClick, AudioChannels.sfx);
    }

    public static class AudioChannels {
        public static string music = "Music";
        public static string sfx = "SFX";
    }
}