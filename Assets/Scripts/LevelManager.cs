using UnityEngine;
using USCG.Core.Telemetry;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private GameObject grid;

    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private GameObject cellPrefab;
    public List<GridCell> cells;

    [SerializeField] private GameObject activityPrefab;

    [SerializeField] private GameObject eventScreen;

    [SerializeField] private List<float> resources;

    [SerializeField] private TaskList requiredTaskList;
    [SerializeField] private TaskList bonusTaskList;

    public TimeHand timeHand;

    [Header("Telemetry")]
    [SerializeField] bool doPlannerMetric = true;

    public bool pauseMenuInteractible = true;
    public bool levelIsActive = true;

    [Header("End Scenes")]
    public EndSceneScreen victory;
    public EndSceneScreen win;
    public EndSceneScreen lose;
    public EndSceneScreen activeEndScreen;

    protected override bool IsPersistent () { return false; }

    IEnumerator PrepareCells(Week currentWeek, List<GridCell> cells) {
        for(int i = 0; i < currentWeek.days; i++) {
            GameObject column = Instantiate(columnPrefab);
            column.name = "Column " + (i+1);
            column.transform.SetParent(grid.transform);

            string dayName = null;
            switch(i) { case 0: dayName = "SUN"; break; case 1: dayName = "MON"; break; case 2: dayName = "TUE"; break; case 3: dayName = "WED"; break; case 4: dayName = "THU"; break; case 5: dayName = "FRI"; break; case 6: dayName = "SAT"; break; }
            column.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dayName;

            timeHand.startPositions.Add(column.transform.GetChild(0).GetChild(0).gameObject);
            GlobalGameManager.SendThemeUpdate();
            for(int j = currentWeek.dayStartHour; j < currentWeek.hoursPerDay + currentWeek.dayStartHour; j++) {
                GameObject cell = Instantiate(cellPrefab);
                cell.name = "Cell " + (j);
                cell.transform.SetParent(column.transform);
                GridCell cellComponent = cell.GetComponent<GridCell>();
                if(cellComponent != null) {
                    cellComponent.day = i + 1;
                    cellComponent.hour = j;
                    cells.Add(cellComponent);
                    cell.GetComponent<SimpleMenuObject>().OnThemeUpdate();
                    cell.GetComponent<TextMenuObject>().OnThemeUpdate();
                    //SoundManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().buttonClick, SoundManager.AudioChannels.sfx);
                    yield return new WaitForSeconds(Mathf.Log(1.08f, (float) ((i*17) + Mathf.Max(j, 1))));
                }
            }
        }

        timeHand.StartTimeHand();

        if(currentWeek.fixedActivities.Length > 0) {
            for(int i = 0; i < currentWeek.fixedActivities.Length; i++) {
                ActivityWithTime activeActivity = currentWeek.fixedActivities[i];
                CreateNewFixedActivity(activeActivity.activity, (int) activeActivity.time.x, (int) activeActivity.time.y);
                yield return new WaitForSeconds(Mathf.Log(1.08f, (i+2)));
            }
        }

        if(currentWeek.fixedEvents.Length > 0) {
            for(int i = 0; i < currentWeek.fixedEvents.Length; i++) {
                EventWithTime activeEvent = currentWeek.fixedEvents[i];
                GridCell targetCell = cells[GetGridCellIndex((int) activeEvent.time.x, (int) activeEvent.time.y)];
                targetCell.occupyingEvent = activeEvent.eventObject;
                targetCell.GetComponent<TextMeshProUGUI>().text = "! EVENT !";
                targetCell.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
                //targetCell.GetComponent<TextMeshProUGUI>().color = ;

                yield return new WaitForSeconds(Mathf.Log(1.08f, (i + 2)));
            }
        }

        yield return new WaitForSeconds(0.5f);
        if(currentWeek.tutorialContent.Length < 1) {
            Instance.levelIsActive = true;
        } else {
            GlobalGameManager.AddScene("Tutorial");
        }

        GlobalGameManager.SendThemeUpdate();
    }

    IEnumerator PrepareResources(Week currentWeek) {
        resources = new List<float> { GlobalGameManager.GetCurrentWeekIndex() };
        if(currentWeek.resourceBars.Length > 1) {
            for(int i = 1; i < currentWeek.resourceBars.Length; i++) {
                resources.Add(currentWeek.resourceBars[i].startingValue);
            }
        }
        yield return null;
    }

    public void StartLevel() {
        Instance.levelIsActive = false;
        Week currentWeek = GlobalGameManager.GetCurrentWeek();

        cells = new List<GridCell>();
        StartCoroutine(PrepareCells(currentWeek, cells));
        StartCoroutine(PrepareResources(currentWeek));

        //_spaceBarMetric = TelemetryManager.instance.CreateAccumulatedMetric("SpaceBarMetric");
        _plannerMetric = TelemetryManager.instance.CreateSampledMetric<string>("PlannerMetric");
    }

    public static void PauseScene() {
        GlobalGameManager.AddScene("PauseMenu");
        Instance.levelIsActive = false;
        MusicManager.Instance.MuffleMusicIfInLevel(true);
    }

    public static void UnPauseScene() {
        GlobalGameManager.CloseScene("PauseMenu");
        Instance.levelIsActive = true;
        MusicManager.Instance.MuffleMusicIfInLevel(false);
    }

    public void TogglePause() {
        if(pauseMenuInteractible) {
            if(levelIsActive) { PauseScene(); }
            else { UnPauseScene(); }
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
        return ((day - 1) * GlobalGameManager.GetCurrentWeek().hoursPerDay) + (hour - GlobalGameManager.GetCurrentWeek().dayStartHour);
        //Examples: Day 1, Cell 6 (index 0): ((1-1)*17)+(6-6) = 0;   Day 2, Cell 21 (index 32): ((2-1)*17)+(21-6) = 32;   Day 7, Cell 22 (index 118): ((7-1)*17)+(22-6) = 118
    }

    public GridCell GridCellFromIndex(int day, int hour) {
        return cells[GetGridCellIndex(day, hour)];
    }

    public static void SetResource(int index, float value) { Instance.resources[index] = value; }
    public static float GetResource(int index) { return Instance.resources[index]; }

    // Telemetry //

    private MetricId _plannerMetric = default;

    public void SamplePlannerMetric(int day, int hour) {
        if(doPlannerMetric) {
            //Debug.Log("Creating planner sample...");
            string plannerData = "\nWeek " + GlobalGameManager.GetCurrentWeekIndex()+1 + " Day " + day + " Hour " + hour + "; Happiness = " + GetResource(1) + " Money = " + GetResource(2) + "\n";
            for(int i = 0; i < cells.Count; i++) {
                string occupyingActivity = "";
                if(cells[i].occupyingActivity != null) { occupyingActivity += cells[i].occupyingActivity.initializer.activity.title;  } else { occupyingActivity += "null"; }
                plannerData += "cell_" + i.ToString("000") + ": " + occupyingActivity + ", ";
            }

            TelemetryManager.instance.AddMetricSample(_plannerMetric, plannerData);
        }
        //TelemetryManager.instance.AccumulateMetric(_spaceBarMetric, 1);
    }
}