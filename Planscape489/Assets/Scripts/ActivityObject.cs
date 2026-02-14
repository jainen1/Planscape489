using UnityEngine;

[CreateAssetMenu(fileName = "ActivityObject", menuName = "Scriptable Objects/ActivityObject")]
public class ActivityObject : ScriptableObject
{
    public string title;
    public int length;
    public ActivityType type;
    public int happiness;
    public int money;
}