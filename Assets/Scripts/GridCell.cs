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
}