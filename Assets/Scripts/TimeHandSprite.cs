using UnityEngine;
using TMPro;

public class TimeHandSprite : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] Vector3 origin;
    [HideInInspector] public float timer;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private AudioClip[] clockTicking;
    private int clockTickIndex;
    [SerializeField] private Vector3[] dayStartPositions;

    [Header("Fast Forward")]
    [SerializeField] private bool isFast = false;
    [SerializeField] private float fastSpeedModifier = 1.5f;

    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        //origin = gameObject.transform.position;
        gameObject.transform.position = dayStartPositions[0];
        timer = GlobalGameManager.Instance.GetCurrentWeek().firstPreparationTime;
        clockTickIndex = 0;
    }

    private void Update() {
        timerObject.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00");
        if(levelManager.levelIsActive) {
            if(timer > 0) {
                timer = Mathf.Max(0, timer - Time.deltaTime);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (GlobalGameManager.Instance.GetCurrentWeek().timeHandSpeed * Time.deltaTime * (isFast? fastSpeedModifier : 1)), gameObject.transform.position.z);
            }
        }
    }

    public bool IsFast() {
        return isFast;
    }

    public void IsBecomeFast(bool yes) {
        isFast = yes;
    }

    /*private void OnTriggerEnter2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();
        Activity activity = collision.GetComponent<Activity>();

        if(cell != null && cell.canBeUsed) {
            AudioSource.PlayClipAtPoint(clockTicking[clockTickIndex], origin, 1.0f);
            clockTickIndex = (clockTickIndex > clockTicking.Length - 2) ? 0 : clockTickIndex + 1;

            if(cell.occupyingActivity != null) {
                cell.occupyingActivity.initializer.SetFixed(true);
            }
            cell.isFixed = true;
            cell.GetComponent<MenuObject>().UpdateMenuObject();
        } else if(activity != null) {
            levelManager.SetHappiness(Mathf.Min(levelManager.GetHappiness() + activity.initializer.activity.happiness, 100));
            levelManager.SetMoney(levelManager.GetMoney() + activity.initializer.activity.money);

            if(levelManager.GetHappiness() < 0 || levelManager.GetMoney() < 0) {
                levelManager.LoseScene();
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();

        if(cell != null && cell.canBeUsed) {
            AudioSource.PlayClipAtPoint(clockTicking[clockTickIndex], Camera.main.transform.position, 1.0f);
            clockTickIndex = (clockTickIndex > clockTicking.Length - 2) ? 0 : clockTickIndex + 1;

            float finalHappiness = levelManager.GetHappiness();
            float finalMoney = levelManager.GetMoney();

            if(cell.occupyingActivity != null) {
                cell.occupyingActivity.initializer.SetFixed(true);

                finalHappiness += cell.occupyingActivity.initializer.activity.happiness;
                finalMoney += cell.occupyingActivity.initializer.activity.money;
            }

            //Debug.Log("Final Stuff being evaluated");


            if(finalHappiness > 150) { finalHappiness -= Mathf.Min(finalHappiness - 150, 10); }
            else if(finalHappiness > 100) { finalHappiness -= Mathf.Min(finalHappiness - 100, 5); }

            levelManager.SetHappiness(Mathf.Min(finalHappiness, 200));
            levelManager.SetMoney(finalMoney);

            if(levelManager.GetHappiness() <= 0 || levelManager.GetMoney() < 0) {
                levelManager.LoseScene();
            }

            cell.isFixed = true;
            cell.GetComponent<MenuObject>().UpdateMenuObject();

            levelManager.SamplePlannerMetric(cell.day, cell.hour);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();
        if(cell != null && cell.hour == 22) {
            if(cell.day == 7) {
                if(GlobalGameManager.Instance.GetCurrentWeekIndex() == 15) { //replace with a "GlobalGameManager.Instance.GetLastWeekIndex()
                    levelManager.WinScene();
                } else {
                    levelManager.VictoryScene();
                }
                Destroy(gameObject);
            } else {
                gameObject.transform.position = dayStartPositions[cell.day];
                timer = GlobalGameManager.Instance.GetCurrentWeek().dailyPreparationTime;
            }
        }
    }
}