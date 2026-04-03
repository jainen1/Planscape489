using TMPro;
using UnityEngine;

public class TaskListItem : MonoBehaviour
{
    private LevelManager gameManager;
    [SerializeField] private GameObject activity;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip rejectSound;

    private GameObject newActivity;

    [HideInInspector] public ActivityWithCount activityWithCount;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject resourceTextComponent;
    [SerializeField] private GameObject countComponent;
    [SerializeField] private GameObject countComponentBackground;


    [SerializeField] private GameObject viewport;

    [SerializeField] TaskList taskList;

    [SerializeField] private bool isVisible;
    [SerializeField] private int count;

    private void Awake() {
        gameManager = FindFirstObjectByType<LevelManager>();

        isVisible = GetComponent<BoxCollider2D>().IsTouching(viewport.GetComponent<BoxCollider2D>());
    }

    public void OnMouseDown() {
        if(!gameManager.isPaused() && isVisible) {
            if(count > 0) {
                AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 1.0f);

                //newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                newActivity = Instantiate(activity, gameObject.transform.position, Quaternion.identity);

                newActivity.GetComponent<ActivityInitializer>().activity = activityWithCount.activity;
                newActivity.GetComponent<ActivityInitializer>().activityType = taskList.GetActivityType();
                newActivity.GetComponent<ActivityInitializer>().Initialize();
                newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseDown", SendMessageOptions.RequireReceiver);

                SetCount(count - 1);
            } else {
                AudioSource.PlayClipAtPoint(rejectSound, Camera.main.transform.position, 1.0f);
            }
        }
    }

    public void OnMouseUp() {
        if(!gameManager.isPaused() && isVisible) {
            newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseUp", SendMessageOptions.RequireReceiver);
        }
    }

    public void Initialize() {
        title.GetComponent<TextMeshProUGUI>().text = activityWithCount.activity.title;

        string resourceText = string.Empty;
        if(activityWithCount.activity.happiness != 0) { resourceText += (activityWithCount.activity.happiness >= 0 ? "H+" : "H-") + Mathf.Abs(activityWithCount.activity.happiness * activityWithCount.activity.length); }
        if(activityWithCount.activity.happiness != 0) { resourceText += "\n" + (activityWithCount.activity.money >= 0 ? "$+" : "$-") + Mathf.Abs(activityWithCount.activity.money * activityWithCount.activity.length); }
        resourceTextComponent.GetComponent<TextMeshProUGUI>().text = resourceText;

        SetCount(activityWithCount.count);
    }

    public int GetCount() {
        return count;
    }

    public void SetCount(int newCount) {
        count = newCount;

        if(count <= 0) {
            countComponentBackground.transform.localScale = Vector3.zero;

        } else {
            countComponentBackground.transform.localScale = Vector3.one;
        }

        gameObject.GetComponent<MenuObject>().UpdateMenuObject();

        countComponent.GetComponent<TextMeshProUGUI>().text = count.ToString("##");
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        
    }

    public void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject == viewport) {
            isVisible = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject == viewport) {
            isVisible = false;
        }
    }
}