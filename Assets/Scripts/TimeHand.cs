using UnityEngine;
using TMPro;
using System.Collections;

public class TimeHand : MonoBehaviour {
    [HideInInspector] public float timer;
    [SerializeField] private GameObject timerObject;
    private int clockTickIndex;
    [SerializeField] private GameObject[] startPositions;

    [Header("Fast Forward")]
    [SerializeField] private bool isFast = false;
    [SerializeField] private float fastSpeedModifier = 1.5f;

    void Start() {
        StartCoroutine(TeleportAfterDelay());
        timer = GlobalGameManager.GetCurrentWeek().firstPreparationTime;
        clockTickIndex = 0;
    }

    private void Update() {
        timerObject.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00");
        if(LevelManager.Instance.levelIsActive) {
            if(timer > 0) {
                timer = Mathf.Max(0, timer - Time.deltaTime);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (GlobalGameManager.GetCurrentWeek().timeHandSpeed * Time.deltaTime * (isFast? fastSpeedModifier : 1)), gameObject.transform.position.z);
            }
        }
    }

    IEnumerator TeleportAfterDelay () {
        yield return null;
        gameObject.transform.position = new Vector3(startPositions[0].transform.position.x, startPositions[0].transform.position.y, -2);
    }

    public bool IsFast() {
        return isFast;
    }

    public void IsBecomeFast(bool yes) {
        isFast = yes;
        gameObject.GetComponent<SimpleMenuObject>().OnThemeUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();

        if(cell != null && cell.canBeUsed) {
            SoundManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().clockTicking[clockTickIndex], SoundManager.AudioChannels.sfx);
            clockTickIndex = (clockTickIndex > GlobalGameManager.GetCurrentMenuTheme().clockTicking.Length - 2) ? 0 : clockTickIndex + 1;

            float finalHappiness = LevelManager.GetResource(1);
            float finalMoney = LevelManager.GetResource(2);

            if(cell.occupyingEvent != null) {
                Debug.Log(cell.occupyingEvent.title + ": " + cell.occupyingEvent.description);
            }

            if(cell.occupyingActivity != null) {
                cell.occupyingActivity.initializer.SetFixed(true);

                finalHappiness += cell.occupyingActivity.initializer.activity.happiness;
                finalMoney += cell.occupyingActivity.initializer.activity.money;
            }

            //Debug.Log("Final Stuff being evaluated");

            if(finalHappiness > 150) { finalHappiness -= Mathf.Min(finalHappiness - 150, 10); }
            else if(finalHappiness > 100) { finalHappiness -= Mathf.Min(finalHappiness - 100, 5); }

            LevelManager.SetResource(1, Mathf.Min(finalHappiness, 200));
            LevelManager.SetResource(2, finalMoney);

            if(LevelManager.GetResource(1) <= 0 || LevelManager.GetResource(2) < 0) {
                LevelManager.Instance.LoseScene();
                Destroy(gameObject);
            }

            cell.isFixed = true;
            cell.GetComponent<SimpleMenuObject>().OnThemeUpdate();

            LevelManager.Instance.SamplePlannerMetric(cell.day, cell.hour);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();
        if(cell != null && cell.hour == 22) {
            if(cell.day == 7) {
                if(LevelManager.Instance.RequiredTaskListIsEmpty()) {
                    if(GlobalGameManager.GetCurrentWeekIndex() == GlobalGameManager.GetLastWeekIndex() - 1) {
                        LevelManager.Instance.VictoryScene();
                    } else {
                        LevelManager.Instance.WinScene();
                    }
                } else { LevelManager.Instance.LoseScene(); }
                Destroy(gameObject);
            } else {
                gameObject.transform.position = new Vector3(startPositions[cell.day].transform.position.x, startPositions[cell.day].transform.position.y, -2f);
                timer = GlobalGameManager.GetCurrentWeek().dailyPreparationTime;
            }
        }
    }
}