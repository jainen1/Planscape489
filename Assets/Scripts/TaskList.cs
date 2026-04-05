using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    private LevelManager gameManager;

    [SerializeField] private GameObject listItem;

    [SerializeField] private GameObject firstItem;

    [SerializeField] private List<GameObject> itemList;

    [SerializeField] private ActivityType activityType;

    private void Awake() {
        itemList = new List<GameObject>();

        ActivityWithCount[] activities;

        gameManager = FindFirstObjectByType<LevelManager>();

        switch(activityType) {
            case ActivityType.Required: activities = gameManager.week.requiredTasks; break;
            case ActivityType.Bonus: activities = gameManager.week.bonusTasks; break;
            default: activities = new ActivityWithCount[0]; break;
        }

        CreateList(activities);
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

    public ActivityType GetActivityType() { return activityType; }
    public void SetActivityType(ActivityType type) { activityType = type; }

    public void CreateList(ActivityWithCount[] activities) {
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

    public void AddTaskListItem(ActivityWithCount activity) {
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