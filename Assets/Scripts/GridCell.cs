using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool canBeUsed;

    public int day;
    public int hour;

    public Activity occupyingActivity;
    public Activity occupyingFoodActivity;
    public bool isFixed;

    public EventObject occupyingEvent;

    void Awake()
    {
        occupyingActivity = null;
        occupyingFoodActivity = null;

        if(canBeUsed) {
            hour = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
            day = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1));
        }
    }
}