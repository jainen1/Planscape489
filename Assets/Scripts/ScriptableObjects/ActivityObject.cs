using UnityEngine;

[CreateAssetMenu(fileName = "ActivityObject", menuName = "Scriptable Objects/ActivityObject")]
public class ActivityObject : StuffedScriptableObject
{
    public string title;
    public int length;
    public int fullStomachLength;
    public int happiness;
    public int money;

    [Header("Audio Settings")]
    public AudioClip sound;

    [Range(0.1f, 3.0f)]
    public float pitch = 1.0f;
}