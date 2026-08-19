using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;

    //track current activity audio source and fade coroutine to allow for stopping and fading out previous activity sounds when a new one is played
    private static AudioSource currentActivityAudioSource;
    private static Coroutine currentFadeCoroutine;

    public static float fadeDuration = 0.5f; // Duration for fading out the previous activity sound

    public static AudioMixer GetAudioMixer() { return Instance.audioMixer; }

    public static void PlayClip(AudioClip clip, [UnityEngine.Internal.DefaultValue("SFX")] string channelName) {
        // from here on is an edited version of the 'AudioSource.PlayClipAtPoint' function. 'AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Pow(10f, volume / 20));'
        GameObject temporaryAudioObject = new GameObject("TemporaryAudio(" + clip.name + ")");
        temporaryAudioObject.transform.position = Camera.main.transform.position;
        //DontDestroyOnLoad(temporaryAudioObject);
        temporaryAudioObject.transform.SetParent(Instance.gameObject.transform);

        AudioSource audioSource = (AudioSource) temporaryAudioObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = Instance.audioMixer.FindMatchingGroups(channelName)[0]; //previously 'audioSource.volume = Mathf.Pow(10f, volume / 20);'
        audioSource.bypassEffects = true;
        audioSource.Play();
        Destroy(temporaryAudioObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public static void PlayClickSound() {
        if(GlobalGameManager.GetCurrentMenuTheme().buttonClick == null) { return; }
        PlayClip(GlobalGameManager.GetCurrentMenuTheme().buttonClick, AudioChannels.sfx);
    }

    public static class AudioChannels {
        public static string music = "Music";
        public static string sfx = "SFX";
    }

    public static void DiegeticActivitySound(AudioClip activityClip, float pitch = 1.0f) {


        //0 - BC: check if activity exists 
        if (activityClip == null) { return; }

        //1 - if there is a current activity audio source playing, stop it and fade it out
        if (currentActivityAudioSource != null && currentActivityAudioSource.isPlaying) { //check if there is a current activity audio source playing

            if (currentFadeCoroutine != null) { //check if there is a current fade coroutine running
                Instance.StopCoroutine(currentFadeCoroutine); //stop the current activity fade coroutine
            }
                Instance.StartCoroutine(Instance.FadeOutAndDestroy(currentActivityAudioSource, fadeDuration)); //start the fade out coroutine for the current activity audio source
            }
        

        //3 - make temp game audio object
        GameObject temporaryAudioObject = new GameObject("ActivityAudio(" + activityClip.name + ")");
        temporaryAudioObject.transform.position = Camera.main.transform.position;
        temporaryAudioObject.transform.SetParent(Instance.gameObject.transform);

        //4 -  configure audio source
        AudioSource audioSource = temporaryAudioObject.AddComponent<AudioSource>();
        audioSource.clip = activityClip;
        audioSource.pitch = pitch;
        audioSource.bypassEffects = false;

        //4.5 - reverb filter 
        AudioReverbFilter reverb = temporaryAudioObject.AddComponent<AudioReverbFilter>();
        reverb.reverbPreset = AudioReverbPreset.Room;

        //5 - set audio mixer group
        if (Instance.audioMixer != null)
        {
            var groups = Instance.audioMixer.FindMatchingGroups(AudioChannels.sfx);
            if (groups.Length > 0)
            {
                audioSource.outputAudioMixerGroup = groups[0];
            }
        }

        audioSource.pitch = pitch;
        audioSource.bypassEffects = true;
        audioSource.Play();

        //6 - destroy temp audio object after clip length
            
        float safePitch = Mathf.Max(0.1f, Mathf.Abs(pitch));
        float realClipDuration = activityClip.length / safePitch;
        float timeScale = Time.timeScale < 0.01f ? 0.01f : Time.timeScale;

        Destroy(temporaryAudioObject, (activityClip.length / safePitch) * timeScale); //cleanup
        
    }

    // for fading out the previous activity sound
    private IEnumerator FadeOutAndDestroy(AudioSource sourceToFade, float duration) {
        if (sourceToFade == null) yield break;

        float startVolume = sourceToFade.volume;
        float timer = 0f;

        while (timer < duration) {
            if (sourceToFade == null) yield break; // Safety check if destroyed elsewhere
            
            timer += Time.deltaTime;
            sourceToFade.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        if (sourceToFade != null) {
            sourceToFade.Stop();
            // Allow 0.5s for the reverb tail to finish decaying before destroying the GameObject
            Destroy(sourceToFade.gameObject, 0.5f); 
        }
    }

}

