using UnityEngine;

[CreateAssetMenu(fileName = "Campaign", menuName = "Scriptable Objects/Campaign")]
public class Campaign : StuffedScriptableObject
{
    public Sprite thumbnail;
    public Color accentColor;

    public Week[] weeks;

    public EventObject[] randomEvents;

    [Header("Campaign Menu")]
    public string title; //temp as this will eventually be controlled by the translation key field
    [TextArea(3, 5)]
    public string description;
    public Difficulty difficulty;

    public enum Difficulty{
        Easy,
        Medium,
        Hard
    }
}