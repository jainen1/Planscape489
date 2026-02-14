using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using UnityEngine.EventSystems;

public class Activity : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    private GameManager gameManager;

    [SerializeField] private GameObject shadowPanel;
    [SerializeField] private GameObject fixedBorder;

    private bool isFixed = false;
    public bool isHeld = false;

    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip fixedPickUp;
    [SerializeField] private AudioClip failedPickUp;
    [SerializeField] private AudioClip putDown;
    [SerializeField] private AudioClip trashSound;
    [SerializeField] private float audioVolume;

    public float yOffset;
    public int length = 1;

    private List<GameObject> collidingCells = new List<GameObject>();
    private GameObject closestCell = null;
    private GameObject occupiedCell = null;

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

    public void SetTargetCell(GridCell cell) {
        closestCell = cell.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isFixed) {
            if(CellIsAvailable(collision.GetComponent<GridCell>()) && !collidingCells.Contains(collision.gameObject)) {
                collidingCells.Add(collision.gameObject);
            }

            if(collision.gameObject.GetComponent<TaskDelete>() != null) {
                isTouchingTrashCan = true;
            }
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

    private bool CellIsAvailable(GridCell cell) {
        return cell != null && cell.canBeUsed && gameManager.GetCellStatus(this, cell) && (cell.hour + length < 24);
    }

    public void SetFixed(bool x) {
        isFixed = x;
        fixedBorder.SetActive(x);
    }

    public bool GetFixed() {
        return isFixed;
    }

public void OnMouseDown() {
        if(!isFixed) {
            isHeld = true;
            AudioSource.PlayClipAtPoint(pickUp, gameObject.transform.position, audioVolume);
        } else {
            AudioSource.PlayClipAtPoint(fixedPickUp, gameObject.transform.position, audioVolume);
        }
    }

    public void OnMouseUp() {
        if(!isFixed) {
            if(closestCell == null) {
                foreach(GridCell cell in gameManager.cells) {
                    if(CellIsAvailable(cell)) {
                        SetTargetCell(cell);
                        break;
                    }
                }
            }
            if(closestCell == null) {
                AudioSource.PlayClipAtPoint(failedPickUp, gameObject.transform.position, audioVolume);
                Destroy(gameObject.transform.parent.gameObject);
            } else {
                isHeld = false;

                ClaimCells();
                if(isTouchingTrashCan) {
                    AudioSource.PlayClipAtPoint(trashSound, gameObject.transform.position, audioVolume);
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else {
                    AudioSource.PlayClipAtPoint(putDown, gameObject.transform.position, audioVolume);
                }
            }
        }
    }

    void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void ClaimCells() {
        if(occupiedCell != null) { gameManager.FreeCells(this, occupiedCell.GetComponent<GridCell>()); }
        occupiedCell = closestCell;
        gameManager.OccupyCells(this, closestCell.GetComponent<GridCell>());
    }

    // Update is called once per frame
    void Update() {
        if(isHeld) {
            foreach(GameObject cell in collidingCells) {
                if(closestCell == null || Vector2.Distance(gameObject.transform.position, cell.transform.position) < Vector2.Distance(gameObject.transform.position, closestCell.transform.position)) {
                    closestCell = cell;
                }
            }
        }

        Vector3 targetPosition;
        if(isHeld && !isFixed) {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = -2f;
            gameObject.transform.position = targetPosition;
            //gameObject.transform.position = Vector3.Lerp(transform.position, worldPosition, 8f * Time.deltaTime);
        } else {
            targetPosition = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y, -2f);
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, 5f * Time.deltaTime);
        }

        if(closestCell == null) {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = gameObject.transform.position;
        }
        else {
            shadowPanel.GetComponent<ShadowPanel>().targetPosition = new Vector3(closestCell.transform.position.x, closestCell.transform.position.y, closestCell.transform.position.z - 0.5f);
        }
    }
}