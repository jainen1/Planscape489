using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    private LevelManager gameManager;

    [SerializeField] private GameObject listItem;

    [SerializeField] private GameObject firstItem;

    private List<GameObject> itemList;

    [SerializeField] private ActivityType type;

    private void Awake() {
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
    }

    public void OnItemCreate(ActivityWithCount[] activities) {
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
                GameObject listItemClone = Instantiate(firstItem);
                listItemClone.transform.SetParent(content.transform); // sub object set to content 
                itemList.Add(listItemClone);
                RectTransform t = itemList[i - 1].GetComponent<RectTransform>();// get the location of the previous item 
                listItemClone.GetComponent<RectTransform>().localPosition = new Vector3(t.localPosition.x, t.localPosition.y - t.rect.height - 0.05f, t.localPosition.z); // place the current item below the previous item
                listItemClone.GetComponent<RectTransform>().localScale = Vector3.one;

                listItemClone.GetComponent<TaskListItem>().activityWithCount = activities[i];
                listItemClone.GetComponent<TaskListItem>().Initialize();
            }

            // update the content height 
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, (itemList.Count * 0.45f) + ((itemList.Count-1) * 0.05f));
            //content.GetComponent<RectTransform>().localPosition = ;
            for(int k = 0; k < itemList.Count; k++) {
                RectTransform transform = itemList[k].GetComponent<RectTransform>();
                itemList[k].GetComponent<RectTransform>().localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (((transform.rect.height + 0.05f) * 0.5f) * (itemList.Count - 1)), transform.localPosition.z);
            }
        }
        else {
            firstItem.SetActive(false);
        }
    }
}