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
<<<<<<< Updated upstream
        gameManager = FindFirstObjectByType<LevelManager>();
        index = 0;
=======
        itemList = new List<GameObject>();

        ActivityWithCount[] activities;

        gameManager = FindFirstObjectByType<LevelManager>();

        switch(type) {
            case ActivityType.Daily: activities = gameManager.week.dailyTasks; break;
            case ActivityType.Weekly: activities = gameManager.week.weeklyTasks; break;
            case ActivityType.Bonus: activities = gameManager.week.bonusTasks; break;
            default: activities = new ActivityWithCount[0]; break;
        }

        OnItemCreate(activities);
>>>>>>> Stashed changes
    }

    public void OnMouseDown() {
        if(!gameManager.paused) {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1.0f);

            newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

<<<<<<< Updated upstream
            ActivityWithCount[] activities;
            switch(type) {
                case ActivityType.Daily: activities = gameManager.week.dailyTasks; break;
                case ActivityType.Weekly: activities = gameManager.week.weeklyTasks; break;
                case ActivityType.Bonus: activities = gameManager.week.bonusTasks; break;
                default: activities = new ActivityWithCount[0]; break;
=======
        // generate _countitem 
        if(activities.Length > 0) {
            firstItem.SetActive(true);// the first item instance has been placed in the first position of the list and directly activate
            firstItem.GetComponent<TaskListItem>().activityWithCount = activities[0];
            firstItem.GetComponent<TaskListItem>().Initialize();

            itemList.Add(firstItem);
            int i = 1;
            while(i < activities.Length) {
                GameObject listItemClone = Instantiate(firstItem);
                listItemClone.transform.SetParent(content.transform); // sub object set to content 
                itemList.Add(listItemClone);
                RectTransform t = itemList[i - 1].GetComponent<RectTransform>();// get the location of the previous item 
                listItemClone.GetComponent<RectTransform>().localPosition = new Vector3(t.localPosition.x, t.localPosition.y - t.rect.height - 0.05f, t.localPosition.z); // place the current item below the previous item 
                listItemClone.GetComponent<RectTransform>().localScale = Vector3.one;

                listItemClone.GetComponent<TaskListItem>().activityWithCount = activities[i];
                listItemClone.GetComponent<TaskListItem>().Initialize();
                i++;
>>>>>>> Stashed changes
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