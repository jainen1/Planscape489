using UnityEngine;

[CreateAssetMenu(fileName = "Campaign", menuName = "Scriptable Objects/Campaign")]
public class Campaign : StuffedScriptableObject
{
    public Week[] weeks;

    public EventObject[] randomEvents;
}