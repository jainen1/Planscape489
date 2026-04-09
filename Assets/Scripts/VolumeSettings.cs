using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public AudioMixer myMixer;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;

    public void SetMusicVolume(float value) 
    {
        myMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20);
    }
}