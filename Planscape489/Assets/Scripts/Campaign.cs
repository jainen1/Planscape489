using UnityEngine;

[CreateAssetMenu(fileName = "Campaign", menuName = "Scriptable Objects/Campaign")]
public class Campaign : ScriptableObject
{
    public Week[] weeks;

    public Event[] randomEvents;
}
