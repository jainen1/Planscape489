using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskList : MonoBehaviour, ReceivesThemeUpdates
{
    [Header("Objects")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject[] scrollbar;

    [Header("Parameters")]
    [SerializeField] private GameObject firstItem;
    [SerializeField] private List<GameObject> itemList;
    [SerializeField] private Activity.Type activityType;

    private void Awake() {
        itemList = new List<GameObject>();

        Week.Utilities.ActivityWithCount[] activities;

        switch(activityType) {
            case Activity.Type.Required: activities = GlobalGameManager.GetCurrentWeek().requiredTasks; break;
            case Activity.Type.Bonus: activities = GlobalGameManager.GetCurrentWeek().bonusTasks; break;
            default: activities = new Week.Utilities.ActivityWithCount[0]; break;
        }
        CreateList(activities);
    }

    void OnEnable () { GlobalGameManager.OnUpdateTheme += OnThemeUpdate; }
    void OnDisable () { GlobalGameManager.OnUpdateTheme -= OnThemeUpdate; }

    public void OnThemeUpdate () {
        int activityTypeIndex;

        switch(activityType) {
            case Activity.Type.Required: activityTypeIndex = 0; break;
            case Activity.Type.Bonus: activityTypeIndex = 2; break;
            default: activityTypeIndex = 0; break;
        }

        MenuTheme.TaskListColors colors = GlobalGameManager.GetCurrentMenuTheme().taskListColors[activityTypeIndex];
        main.GetComponent<SpriteRenderer>().color = colors.mainColor;
        foreach(GameObject scrollbarObject in scrollbar) {
            scrollbarObject.GetComponent<Image>().color = colors.scrollbarColor;
        }

        foreach(GameObject taskListItem in itemList) {
            if(taskListItem.GetComponent<TaskListItem>().GetCount() == 0) {
                taskListItem.GetComponent<Image>().color = GlobalGameManager.GetCurrentMenuTheme().fixedActivityColor;
            } else { taskListItem.GetComponent<Image>().color = colors.itemColor; }
            taskListItem.GetComponent<TaskListItem>().countComponentBackground.GetComponent<Image>().color = colors.countColor;
        }
    }

    public Color GetMainColor () {
        return GlobalGameManager.GetCurrentMenuTheme().taskListColors[0].mainColor;
    }

    public bool ReturnTaskToList(ActivityObject activity) {
        foreach(GameObject item in itemList) {
            if(item != null) {
                TaskListItem script = item.GetComponent<TaskListItem>();
                if(script != null && script.activityWithCount.activity == activity) {
                    script.SetCount(script.GetCount()+1);
                    return true;
                }
            }
        } return false;
    }

    public bool TaskListIsEmpty() {
        foreach(GameObject item in itemList) {
            if(item != null) {
                TaskListItem script = item.GetComponent<TaskListItem>();
                if(script != null && script.GetCount() > 0) {
                    return false;
                }
            }
        }
        return true;
    }

    public Activity.Type GetActivityType() { return activityType; }
    public void SetActivityType(Activity.Type type) { activityType = type; }

    public void CreateList(Week.Utilities.ActivityWithCount[] activities) {
        GameObject content = gameObject;

        // destroys the previously generated item and clears the list 
        for(int i = 1; i < itemList.Count; i++) {
            Destroy(itemList[i]);
        }
        itemList.Clear();

        // generate _countitem 
        if(activities.Length > 0) {
            firstItem.SetActive(true);// the first item instance has been placed in the first position of the list and directly activate
            firstItem.GetComponent<TaskListItem>().activityWithCount = activities[0];
            firstItem.GetComponent<TaskListItem>().Initialize();

            itemList.Add(firstItem);
            for(int i = 1; i < activities.Length; i++) {
                AddTaskListItem(activities[i]);
            }

            float taskItemHeight = 0.45f;
            float taskItemDistance = 0.05f;

            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, (activities.Length * taskItemHeight) + ((activities.Length - 1) * taskItemDistance)); // update the content height

            for(int i = 1; i < activities.Length; i++) {
                Vector3 previousPosition = itemList[Mathf.Max(0, i - 1)].GetComponent<RectTransform>().localPosition;// get the location of the previous item 
                itemList[i].GetComponent<RectTransform>().localPosition = new Vector3(previousPosition.x, previousPosition.y - (taskItemHeight + taskItemDistance), previousPosition.z); // place the current item below the previous item
            }

            for(int i = 0; i < activities.Length; i++) {
                Vector3 currentPosition = itemList[i].GetComponent<RectTransform>().localPosition;
                itemList[i].GetComponent<RectTransform>().localPosition = new Vector3(currentPosition.x, currentPosition.y + (((taskItemHeight + taskItemDistance) * 0.5f) * (activities.Length - 1)), currentPosition.z);
            }
        }
        else {
            firstItem.SetActive(false);
        }
    }

    public void AddTaskListItem(Week.Utilities.ActivityWithCount activity) {
        GameObject listItemClone = Instantiate(firstItem);
        listItemClone.transform.SetParent(gameObject.transform); // sub object set to content
        itemList.Add(listItemClone);

        listItemClone.GetComponent<TaskListItem>().activityWithCount = activity;
        listItemClone.GetComponent<TaskListItem>().Initialize();

        AdjustTaskListSize();
    }

    public void AdjustTaskListSize() {
        float taskItemHeight = 0.45f;
        float taskItemDistance = 0.05f;

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, (itemList.Count * taskItemHeight) + ((itemList.Count - 1) * taskItemDistance) + taskItemHeight); // update the content height
    }
}