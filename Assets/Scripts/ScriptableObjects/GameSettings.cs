using System;
using UnityEngine;

[Serializable]
public class GameSettings {
    public float musicVolume;
    public float sfxVolume;
    public float mouseSensitivity;

    public GameSettings () {
        this.musicVolume = 1f;
        this.sfxVolume = 1f;
        this.mouseSensitivity = 1f;
    }
}