using UnityEngine;

[CreateAssetMenu(fileName = "Campaign", menuName = "Scriptable Objects/Campaign")]
public class Campaign : StuffedScriptableObject
{
    public string name; //temp as this will eventually be controlled by the translation key field
    [TextArea(4, 6)]
    public string description;

    public Sprite thumbnail;

    public Week[] weeks;

    public EventObject[] randomEvents;
}