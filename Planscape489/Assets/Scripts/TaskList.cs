using UnityEngine;

public class TaskList : MonoBehaviour
{
    [SerializeField] private GameObject activity;
    [SerializeField] private ActivityObject[] possibleObjects;

    [SerializeField] private AudioClip clickSound;

    private GameObject newActivity;

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);

        newActivity = Instantiate(activity);
        newActivity.GetComponent<ActivityInitializer>().activity = possibleObjects[Random.Range(0, possibleObjects.Length)];
        newActivity.GetComponent<ActivityInitializer>().Initialize();
        newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseDown", SendMessageOptions.RequireReceiver);
    }

    public void OnMouseUp() {
        newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseUp", SendMessageOptions.RequireReceiver);
    }
}