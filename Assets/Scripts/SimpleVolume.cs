using UnityEngine;
using UnityEngine.Audio;

public class SimpleVolume : MonoBehaviour
{
    public AudioMixer myMixer;

    public void ChangeVol(float value)
    {
    //linear: -40dB (quiet) to 0dB (loud)
        myMixer.SetFloat("MusicVol", value);
    }
}