using UnityEngine;

[CreateAssetMenu(fileName = "ActivityObject", menuName = "Scriptable Objects/ActivityObject")]
public class ActivityObject : ScriptableObject
{
    public string title;
    public Color color;
    public int length;

    //public bool isFixed;
}
