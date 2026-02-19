using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MenuTheme menuTheme;

    public GridCell[] cells;

    [SerializeField] private GameObject activityPrefab;

    [SerializeField] private ActivityObject[] fixedActivities;
    [SerializeField] private Vector2[] fixedActivityTimes;

    public delegate void UpdateTheme();
    public static event UpdateTheme OnUpdateTheme;
    public static event UpdateTheme OnLateUpdateTheme;

    private int happiness;
    private int money;

    // Start is called before the first frame update
    void Start()
    {
        SetHappiness(70);
        SetMoney(2000);

        for(int i = 0; i < fixedActivities.Length; i++) {
            CreateNewFixedActivity(fixedActivities[i], (int)fixedActivityTimes[i].x, (int)fixedActivityTimes[i].y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InUpdateTheme() {
        OnUpdateTheme();
        OnLateUpdateTheme();
    }

    private void CreateNewFixedActivity(ActivityObject activity, int day, int hour) {
        GameObject fixedActivity = Instantiate(activityPrefab);
        fixedActivity.GetComponent<ActivityInitializer>().activity = activity;
        fixedActivity.GetComponent<ActivityInitializer>().Initialize();
        fixedActivity.GetComponentInChildren<Activity>().SetTargetCell(cells[GetGridCellIndex(day, hour)]);
        fixedActivity.GetComponent<ActivityInitializer>().SetFixed(true);
        fixedActivity.GetComponentInChildren<Activity>().ClaimCells();
    }

    public bool GetCellStatus(Activity activity, GridCell startCell) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = startCellIndex + activity.initializer.activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            if(endCellIndex > cells.Length - 1 || (cells[i].occupyingActivity != null && cells[i].occupyingActivity != activity)) {
                return false;
            }
        } return true;
    }

    public bool GetCellFoodStatus(Activity activity, GridCell startCell) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = Mathf.Min(startCellIndex + activity.initializer.activity.fullStomachLength - 1, GetGridCellIndex(startCell.day, 22));
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            if(cells[i].occupiedByFood || (cells[i].occupyingActivity != null && cells[i].occupyingActivity != activity)) {
                return false;
            }
        }
        return true;
    }

    public void FreeOrOccupyCells(Activity activity, GridCell startCell, bool free) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = startCellIndex + activity.initializer.activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            cells[i].occupyingActivity = free? null : activity;
        }
        if(activity.initializer.activity.fullStomachLength > 0) {
            // if startCell.hour + activity.initializer.activity.fullStomachLength > 23, food panel will hang
            int foodEndCellIndex = Mathf.Min(startCellIndex + activity.initializer.activity.fullStomachLength - 1, GetGridCellIndex(startCell.day, 22));
            for(int i = startCellIndex; i <= foodEndCellIndex; i++) {
                cells[i].occupiedByFood = free ? false : true;
            }
        }
    }

    public int GetGridCellIndex(int day, int hour) {
        return ((day - 1) * 17) + (hour - 6);
        //Day 1, Cell 6 (index 0): ((1-1)*17)+(6-6) = 0
        //Day 2, Cell 21 (index 32): ((2-1)*17)+(21-6) = 32
        //Day 7, Cell 22 (index 118): ((7-1)*17)+(22-6) = 118
    }

    public GridCell GridCellFromIndex(int day, int hour) {
        return cells[GetGridCellIndex(day, hour)];
    }

    public void SetHappiness(int value) { happiness = value; }
    public int GetHappiness() { return happiness; }

    public void SetMoney(int value) { money = value; }
    public int GetMoney() { return money; }
}