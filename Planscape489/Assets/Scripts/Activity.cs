using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Activity : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject activity;

    public bool isFixed;

    private bool mouseDown;

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("clicked");
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        mouseDown = false;
    }

    void Start() {
        mouseDown = false;
        isFixed = activity.GetComponent<Activity>().isFixed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*if(Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject()) {
            gameObject.transform.position = Input.mousePosition;
        } else {
            gameObject.transform.position = Vector3.Lerp(transform.position, shadow.transform.position, 5f * Time.deltaTime);
        }*/

        if(Input.GetMouseButton(0) && mouseDown && !isFixed) {
            gameObject.transform.position = Input.mousePosition;
        }
        else {
            gameObject.transform.position = Vector3.Lerp(transform.position, shadow.transform.position, 5f * Time.deltaTime);
        }
    }
}