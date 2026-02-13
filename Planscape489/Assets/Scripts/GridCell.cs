using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool canBeUsed;

    public int day;
    public int hour;

    public Activity occupyingActivity;

    void Awake()
    {
        if(canBeUsed) {
            hour = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
            day = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1));
        }
    }

    void Update()
    {
        
    }
}
