using UnityEngine;
using static UnityEngine.UI.Image;

public class TaskList : MonoBehaviour
{
    private LevelManager gameManager;

    [SerializeField] private GameObject activity;

    [SerializeField] private AudioClip clickSound;

    private GameObject newActivity;

    private int index;

    [SerializeField] private ActivityType type;

    private void Awake() {
        gameManager = FindFirstObjectByType<LevelManager>();
        index = 0;
    }

    public void OnMouseDown() {
        if(!gameManager.paused) {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1.0f);

            newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

            ActivityWithCount[] activities;
            switch(type) {
                case ActivityType.Daily: activities = gameManager.week.dailyTasks; break;
                case ActivityType.Weekly: activities = gameManager.week.weeklyTasks; break;
                case ActivityType.Bonus: activities = gameManager.week.bonusTasks; break;
                default: activities = new ActivityWithCount[0]; break;
            }

            newActivity.GetComponent<ActivityInitializer>().activity = activities[index].activity;
            index = (index > activities.Length - 2) ? 0 : index + 1;
            newActivity.GetComponent<ActivityInitializer>().Initialize();
            newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseDown", SendMessageOptions.RequireReceiver);
        }
    }

    public void OnMouseUp() {
        if(!gameManager.paused) {
            newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseUp", SendMessageOptions.RequireReceiver);
        }
    }
}