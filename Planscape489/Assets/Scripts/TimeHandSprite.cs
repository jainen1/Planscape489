using UnityEngine;
using TMPro;

public class TimeHandSprite : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] Vector3 origin;
    [SerializeField] float speed;
    [HideInInspector] public float timer;
    [SerializeField] private float firstPreparationTime = 20;
    [SerializeField] private float dailyPreparationTime = 10;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private AudioClip[] clockTicking;
    private int clockTickIndex;
    [SerializeField] private Vector3[] dayStartPositions;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        //origin = gameObject.transform.position;
        gameObject.transform.position = dayStartPositions[0];
        timer = firstPreparationTime;
        clockTickIndex = 0;
    }

    private void Update() {
        timerObject.GetComponent<TextMeshProUGUI>().text = timer.ToString("00.00");
        if(timer > 0) {
            timer = Mathf.Max(0, timer - Time.deltaTime);
        }
        else {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (speed * Time.deltaTime), gameObject.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
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
            gameManager.SetHappiness(gameManager.GetHappiness() + activity.initializer.activity.happiness);
            gameManager.SetMoney(gameManager.GetMoney() + activity.initializer.activity.money);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();
        if(cell != null && cell.hour == 22) {
            if(cell.day == 7) {

            } else {
                gameObject.transform.position = dayStartPositions[cell.day];
                timer = dailyPreparationTime;
            }
        }
    }
}