using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Activity : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject shadow;

    private bool isFixed;
    private bool mouseDown;

    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip putDown;
    [SerializeField] private float audioVolume;

    public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("clicked");
        mouseDown = true;
        AudioSource.PlayClipAtPoint(pickUp, Vector3.zero, audioVolume);
    }

    public void OnPointerUp(PointerEventData eventData) {
        mouseDown = false;
        AudioSource.PlayClipAtPoint(putDown, Vector3.zero, audioVolume);
    }

    void Start() {
        mouseDown = false;
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

    public void SetFixed(bool x) {
        isFixed = x;
    }
}