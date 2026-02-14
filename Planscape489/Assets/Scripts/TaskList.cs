using UnityEngine;

public class TaskList : MonoBehaviour
{
    [SerializeField] private GameObject activity;
    [SerializeField] private ActivityObject[] possibleObjects;

    [SerializeField] private AudioClip clickSound;

    private GameObject newActivity;

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);

        newActivity = Instantiate(activity, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        newActivity.GetComponent<ActivityInitializer>().activity = possibleObjects[Random.Range(0, possibleObjects.Length)];
        newActivity.GetComponent<ActivityInitializer>().Initialize();
        newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseDown", SendMessageOptions.RequireReceiver);


        MenuObject[] menuObjects = GetComponentsInChildren<MenuObject>();
        foreach(MenuObject menuObject in menuObjects) {
            menuObject.UpdateMenuObject();
        }
    }

    public void OnMouseUp() {
        newActivity.GetComponentInChildren<Activity>().gameObject.SendMessage("OnMouseUp", SendMessageOptions.RequireReceiver);
    }
}