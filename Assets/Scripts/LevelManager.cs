using UnityEngine;
using USCG.Core.Telemetry;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public GridCell[] cells;

    [SerializeField] private GameObject activityPrefab;

    [SerializeField] private GameObject eventScreen;

    [SerializeField] private List<float> resources;

    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [SerializeField] private TaskList requiredTaskList;
    [SerializeField] private TaskList bonusTaskList;

    public TimeHand timeHand;

    [Header("Telemetry")]
    [SerializeField] bool doPlannerMetric = true;

    public bool pauseMenuInteractible = true;
    public bool levelIsActive = true;

    public EndSceneScreen victory;
    public EndSceneScreen win;
    public EndSceneScreen lose;

    public EndSceneScreen activeEndScreen;

    public void StartLevel() {
        Week currentWeek = GlobalGameManager.GetCurrentWeek();

        resources = new List<float> { GlobalGameManager.GetCurrentWeekIndex(), currentWeek.resourceBars[1].startingValue, currentWeek.resourceBars[2].startingValue };
        //resources.Add(currentWeek.resourceBars[1].startingValue);

        if(currentWeek.fixedEvents.Length > 0) {
            for(int i = 0; i < currentWeek.fixedEvents.Length; i++) {
                EventWithTime activeEvent = currentWeek.fixedEvents[i];
                cells[GetGridCellIndex((int) activeEvent.time.x, (int) activeEvent.time.y)].occupyingEvent = activeEvent.eventObject;
            }
        }

        if(currentWeek.fixedActivities.Length > 0) {
            for(int i = 0; i < currentWeek.fixedActivities.Length; i++) {
                ActivityWithTime activeActivity = currentWeek.fixedActivities[i];
                CreateNewFixedActivity(activeActivity.activity, (int) activeActivity.time.x, (int) activeActivity.time.y);
            }
        }
    }

    public void PauseScene() {
        GlobalGameManager.AddScene("PauseMenu");
        levelIsActive = false;
    }

    public void UnPauseScene() {
        GlobalGameManager.CloseScene("PauseMenu");
        levelIsActive = true;
    }

    public void TogglePause() {
        if(pauseMenuInteractible) {
            if(levelIsActive) {
                PauseScene();
            } else {
                UnPauseScene();
            }
        }
    }

    public bool RequiredTaskListIsEmpty() {
        return requiredTaskList.TaskListIsEmpty();
    }

    public void ReturnTaskToList(ActivityObject activity) {
        if(!requiredTaskList.ReturnTaskToList(activity)) {
            bonusTaskList.ReturnTaskToList(activity);
        }
    }

    public void SkipTimer() {
        if(levelIsActive) { timeHand.timer = 0; }
    }

    public void FastForwardTimeHand() { if(levelIsActive) { timeHand.IsBecomeFast(true); } }
    public void NormalSpeedTimeHand() { if(levelIsActive) { timeHand.IsBecomeFast(false); } }
    public void ToggleSpeedTimeHand() { if(levelIsActive) { timeHand.IsBecomeFast(!timeHand.IsFast()); } }

    public void TutorialScene() {
        if(levelIsActive) {
            GlobalGameManager.AddScene("Tutorial");
            pauseMenuInteractible = false;
            levelIsActive = false;
        }
    }

    private void OpenEndScene() {
        pauseMenuInteractible = false;
        levelIsActive = false;

        GlobalGameManager.AddScene("EndScene");
    }

    public void VictoryScene() { activeEndScreen = victory; OpenEndScene(); }
    public void WinScene() { activeEndScreen = win; OpenEndScene(); }
    public void LoseScene() { activeEndScreen = lose; OpenEndScene(); }

    private void CreateNewFixedActivity(ActivityObject activity, int day, int hour) {
        GameObject fixedActivity = Instantiate(activityPrefab);
        ActivityInitializer activityInitializer = fixedActivity.GetComponent<ActivityInitializer>();
        Activity activityScript = fixedActivity.GetComponentInChildren<Activity>();

        activityInitializer.activity = activity;
        activityInitializer.displayFixedBorder = true;
        activityInitializer.Initialize();
        activityScript.SetTargetCell(cells[GetGridCellIndex(day, hour)]);
        activityInitializer.SetFixed(true);
        activityScript.ClaimCells();
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
            if(endCellIndex > cells.Length - 1 || (cells[i].occupyingFoodActivity != null && cells[i].occupyingFoodActivity != activity)) {
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
                cells[i].occupyingFoodActivity = free ? null : activity;
            }
        }
    }

    public static int GetGridCellIndex(int day, int hour) {
        return ((day - 1) * 17) + (hour - 6);
        //Day 1, Cell 6 (index 0): ((1-1)*17)+(6-6) = 0
        //Day 2, Cell 21 (index 32): ((2-1)*17)+(21-6) = 32
        //Day 7, Cell 22 (index 118): ((7-1)*17)+(22-6) = 118
    }

    public GridCell GridCellFromIndex(int day, int hour) {
        return cells[GetGridCellIndex(day, hour)];
    }

    public void SetResource(int index, float value) { resources[index] = value; }
    public float GetResource(int index) { return resources[index]; }

    //Telemetry

    private MetricId _plannerMetric = default;

    private void Start() {
        //_spaceBarMetric = TelemetryManager.instance.CreateAccumulatedMetric("SpaceBarMetric");
        _plannerMetric = TelemetryManager.instance.CreateSampledMetric<string>("PlannerMetric");
    }

    public void SamplePlannerMetric(int day, int hour) {
        if(doPlannerMetric) {
            //Debug.Log("Creating planner sample...");
            string plannerData = "\nWeek " + GlobalGameManager.GetCurrentWeekIndex()+1 + " Day " + day + " Hour " + hour + "; Happiness = " + GetResource(1) + " Money = " + GetResource(2) + "\n";
            for(int i = 0; i < cells.Length; i++) {
                string occupyingActivity = "";
                if(cells[i].occupyingActivity != null) { occupyingActivity += cells[i].occupyingActivity.initializer.activity.title;  } else { occupyingActivity += "null"; }
                plannerData += "cell_" + i.ToString("000") + ": " + occupyingActivity + ", ";
            }

            TelemetryManager.instance.AddMetricSample(_plannerMetric, plannerData);
        }
        //TelemetryManager.instance.AccumulateMetric(_spaceBarMetric, 1);
    }
}