using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Activity : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    [SerializeField] private GameObject shadowPanel;

    private bool isFixed;
    private bool mouseDown;

    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip putDown;
    [SerializeField] private float audioVolume;

    public float yOffset;

    public GameObject closestCell;

    /*public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("clicked");
        mouseDown = true;
        if(isFixed) {
            AudioSource.PlayClipAtPoint(pickUp, transform.position, audioVolume);
        } else {

        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        mouseDown = false;
        if(isFixed) {
            AudioSource.PlayClipAtPoint(putDown, transform.position, audioVolume);
        }
    }*/

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.GetComponent<GridCell>() != null) {
            if(closestCell == null || Vector3.Distance(gameObject.transform.position, collision.transform.position) < Vector3.Distance(gameObject.transform.position, closestCell.transform.position)) {
                closestCell = collision.gameObject;
            }
        }
    }

    public void OnMouseDown() {
        //Debug.Log("clicked");
        mouseDown = true;
        if(isFixed) {
            AudioSource.PlayClipAtPoint(pickUp, transform.position, audioVolume);
        }
        else {

        }
    }

    public void OnMouseUp() {
        mouseDown = false;
        if(isFixed) {
            AudioSource.PlayClipAtPoint(putDown, transform.position, audioVolume);
        }
    }

    void Start() {
        mouseDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject()) {
            gameObject.transform.position = Input.mousePosition;
        } else {
            gameObject.transform.position = Vector3.Lerp(transform.position, shadow.transform.position, 5f * Time.deltaTime);
        }*/

        if(Input.GetMouseButton(0) && mouseDown && !isFixed) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = -2f;
            gameObject.transform.position = worldPosition;
        } else {
            Vector3 targetPosition = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y, -2f);
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, 5f * Time.deltaTime);
        }
    }

    void LateUpdate() {
        if(closestCell == null) {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = gameObject.transform.position;
        } else {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = closestCell.transform.position;
        }
    }

    public void SetFixed(bool x) {
        isFixed = x;
    }
}