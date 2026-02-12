using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    [SerializeField] private GameObject activity;
    [SerializeField] private ActivityObject[] possibleObjects;

    [SerializeField] private AudioClip clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);

        GameObject newActivity = Instantiate(activity);
        newActivity.GetComponent<ActivityInitializer>().activity = possibleObjects[Random.Range(0, possibleObjects.Length)];
        newActivity.GetComponent<ActivityInitializer>().Initialize();
        newActivity.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
