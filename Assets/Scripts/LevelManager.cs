using UnityEngine;
using USCG.Core.Telemetry;

public class LevelManager : MonoBehaviour
{
    //public static new LevelManager Instance;

    public GridCell[] cells;

    [SerializeField] private GameObject activityPrefab;

    [HideInInspector] public Week week;

    private bool playerPaused;
    public bool menuPaused;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject eventScreen;
    [SerializeField] private GameObject tutorialWindow;

    private float happiness;
    private float money;

    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    private float howPaused;

    [SerializeField] private TaskList requiredTaskList;
    [SerializeField] private TaskList bonusTaskList;

    [Header("Telemetry")]
    [SerializeField] bool doPlannerMetric = true;   

    public void StartLevel() {
        /*if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }*/

        week = GlobalGameManager.Instance.GetCurrentWeek();

        SetHappiness(week.startingHappiness);
        SetMoney(week.startingMoney);

        for(int i = 0; i < week.fixedActivities.Length; i++) {
            CreateNewFixedActivity(week.fixedActivities[i].activity, (int) week.fixedActivities[i].time.x, (int) week.fixedActivities[i].time.y);
        }

        SetPauseScene(false);
        tutorialWindow.transform.localScale = Vector3.zero;
    }

    public bool isPaused() {
        return playerPaused || menuPaused;
    }

    public void PauseScene() { SetPauseScene(true); }
    public void UnPauseScene() { SetPauseScene(false); }
    public void TogglePause() { SetPauseScene(!playerPaused); }
    public void SetPauseScene(bool x) {
        if(!menuPaused) {
            playerPaused = x;
            if(x) {
                pauseScreen.transform.localScale = Vector3.one;
            } else {
                pauseScreen.transform.localScale = Vector3.zero;
            }
        }

        /*pauseScreen.GetComponent<CanvasGroup>().interactable = x;
        pauseScreen.GetComponent<CanvasGroup>().alpha = x? 1f : 0f;
        Color pauseScreenColor = pauseScreen.GetComponent<SpriteRenderer>().color;
        pauseScreen.GetComponent<SpriteRenderer>().color = new Color(pauseScreenColor.r, pauseScreenColor.g, pauseScreenColor.b, x? 0.89f : 0f);*/
    }

    public void InitiateTutorial() {
        tutorialWindow.transform.localScale = Vector3.one;
        tutorialWindow.GetComponent<Tutorial>().InitializeTutorial();
    }

    public void ReturnTaskToList(ActivityObject activity) {
        if(!requiredTaskList.ReturnTaskToList(activity)) {
            bonusTaskList.ReturnTaskToList(activity);
        }
    }

    public void SkipTimer() {
        FindFirstObjectByType<TimeHandSprite>().timer = 0;
    }

    public void TutorialScene() {
        menuPaused = true;
        GlobalGameManager.Instance.OpenTutorialScene();
    }

    public void VictoryScene() {
        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, 1.0f);
        menuPaused = true;
        GlobalGameManager.Instance.OpenWinScene();
    }

    public void WinScene() {
        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, 1.0f);
        menuPaused = true;
        GlobalGameManager.Instance.OpenWinScene();
    }

    public void LoseScene() {
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, 1.0f);
        menuPaused = true;
        GlobalGameManager.Instance.OpenLoseScene();
    }

    private void CreateNewFixedActivity(ActivityObject activity, int day, int hour) {
        GameObject fixedActivity = Instantiate(activityPrefab);
        fixedActivity.GetComponent<ActivityInitializer>().activity = activity;
        fixedActivity.GetComponent<ActivityInitializer>().displayFixedBorder = true;
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

    public void SetHappiness(float value) { happiness = value; }
    public float GetHappiness() { return happiness; }

    public void SetMoney(float value) { money = value; }
    public float GetMoney() { return money; }

    //Telemetry

    private MetricId _plannerMetric = default;

    private void Start() {
        //_spaceBarMetric = TelemetryManager.instance.CreateAccumulatedMetric("SpaceBarMetric");
        _plannerMetric = TelemetryManager.instance.CreateSampledMetric<string>("PlannerMetric");
    }

    public void SamplePlannerMetric(int day, int hour) {
        if(doPlannerMetric) {
            //Debug.Log("Creating planner sample...");
            string plannerData = "\nWeek " + week.weekNumber + " Day " + day + " Hour " + hour + "; Happiness = " + GetHappiness() + " Money = " + GetMoney() + "\n";
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