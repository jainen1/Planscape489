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
    [SerializeField] private AudioClip trashSound;
    [SerializeField] private float audioVolume;

    public float yOffset;

    private List<GameObject> collidingCells = new List<GameObject>();
    public GameObject closestCell = null;

    [SerializeField] private bool isTouchingTrashCan = false;

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

    private void OnTriggerEnter2D(Collider2D collision) {
        if(CellIsAvailable(collision) && !collidingCells.Contains(collision.gameObject)) {
            collidingCells.Add(collision.gameObject);
        }

        if(collision.gameObject.GetComponent<TaskDelete>() != null) {
            isTouchingTrashCan=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collidingCells.Contains(collision.gameObject)) {
            collidingCells.Remove(collision.gameObject);
        }

        if(collision.gameObject.GetComponent<TaskDelete>() != null) {
            isTouchingTrashCan = false;
        }
    }

    private bool CellIsAvailable(Collider2D collision) {
        GridCell cell = collision.GetComponent<GridCell>();
        return cell != null && cell.canBeUsed && cell.occupyingActivity != this;
    }

    public void OnMouseDown() {
        mouseDown = true;
        if(!isFixed) {
            AudioSource.PlayClipAtPoint(pickUp, gameObject.transform.position, audioVolume);
        }
    }

    public void OnMouseUp() {
        mouseDown = false;
        if(!isFixed) {
            AudioSource.PlayClipAtPoint(putDown, gameObject.transform.position, audioVolume);
        }

        if(isTouchingTrashCan) {
            AudioSource.PlayClipAtPoint(trashSound, gameObject.transform.position, audioVolume);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    void Start() {
        mouseDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject cell in collidingCells) {
            if(closestCell == null || Vector2.Distance(gameObject.transform.position, cell.transform.position) < Vector2.Distance(gameObject.transform.position, closestCell.transform.position)) {
                closestCell = cell;
            }
        }

        if(Input.GetMouseButton(0) && mouseDown && !isFixed) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = -2f;
            gameObject.transform.position = worldPosition;
        } else {
            Vector3 targetPosition = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y, -2f);
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, 5f * Time.deltaTime);
        }

        if(closestCell == null) {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = gameObject.transform.position;
        }
        else {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = new Vector3(closestCell.transform.position.x, closestCell.transform.position.y, closestCell.transform.position.z - 0.5f);
        }
    }

    public void SetFixed(bool x) {
        isFixed = x;
    }
}