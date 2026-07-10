using UnityEngine;

[CreateAssetMenu(fileName = "ActivityObject", menuName = "Scriptable Objects/ActivityObject")]
public class ActivityObject : StuffedScriptableObject
{
    public string title;
    public int length;
    public int fullStomachLength;
    public int happiness;
    public int money;
}