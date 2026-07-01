using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;

    public void SetMusicVolume(float value)  { audioMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20); }
    public void SetSFXVolume(float value) { audioMixer.SetFloat("SFX Volume", Mathf.Log10(value) * 20); }
}