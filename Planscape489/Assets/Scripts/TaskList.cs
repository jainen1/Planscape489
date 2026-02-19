using UnityEngine;
using static UnityEngine.UI.Image;

public class TaskList : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject activity;
    [SerializeField] private ActivityObject[] possibleObjects;

    [SerializeField] private AudioClip clickSound;

    private GameObject newActivity;

    private int index;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
        index = 0;
    }

    public void OnMouseDown() {
        if(!gameManager.paused) {
            AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);

            newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            newActivity.GetComponent<ActivityInitializer>().activity = possibleObjects[index];
            index = (index > possibleObjects.Length - 2) ? 0 : index + 1;
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