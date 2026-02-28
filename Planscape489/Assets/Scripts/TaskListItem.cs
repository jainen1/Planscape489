using TMPro;
using UnityEngine;

public class TaskListItem : MonoBehaviour
{
    private LevelManager gameManager;
    [SerializeField] private GameObject activity;
    [SerializeField] private AudioClip clickSound;

    private GameObject newActivity;

    [HideInInspector] public ActivityWithCount activityWithCount;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject happinessText;
    [SerializeField] private GameObject moneyText;

    private void Awake() {
        gameManager = FindFirstObjectByType<LevelManager>();
    }

    public void OnMouseDown() {
        if(!gameManager.paused) {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1.0f);

            newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

            newActivity.GetComponent<ActivityInitializer>().activity = activityWithCount.activity;
            newActivity.GetComponent<ActivityInitializer>().Initialize();
            newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseDown", SendMessageOptions.RequireReceiver);
        }
    }

    public void OnMouseUp() {
        if(!gameManager.paused) {
            newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseUp", SendMessageOptions.RequireReceiver);
        }
    }

    public void Initialize() {
        title.GetComponent<TextMeshProUGUI>().text = activityWithCount.activity.title;
        happinessText.GetComponent<TextMeshProUGUI>().text = (activityWithCount.activity.happiness >= 0 ? "H+" : "H-") + Mathf.Abs(activityWithCount.activity.happiness * activityWithCount.activity.length);
        moneyText.GetComponent<TextMeshProUGUI>().text = (activityWithCount.activity.money >= 0 ? "$+" : "$-") + Mathf.Abs(activityWithCount.activity.money * activityWithCount.activity.length);
    }
}