using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    private LevelManager gameManager;
    [SerializeField] public ActivityInitializer initializer;

    public bool isHeld = false;

    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip fixedPickUp;
    [SerializeField] private AudioClip failedPickUp;
    [SerializeField] private AudioClip putDown;
    [SerializeField] private AudioClip trashSound;
    [SerializeField] private float audioVolume;

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
        if(!initializer.IsFixed()) {
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

        //Debug
        /*if(cell == null) { Debug.Log("Cell is null!"); return false; }
        if(!cell.canBeUsed) { Debug.Log("Cell cannot be used!"); return false; }
        if(cell.isFixed) { Debug.Log("Cell is fixed!"); return false; }
        if(!gameManager.GetCellStatus(this, cell)) { Debug.Log("Cell status returned false!"); return false; }
        if(!(cell.hour + initializer.activity.length < 24)) { Debug.Log("Cell is beyond end of day!"); return false; }
        if(!(initializer.activity.fullStomachLength > 0 ? gameManager.GetCellFoodStatus(this, cell) : true)) { Debug.Log("Cell cannot be used for food!");  return false; }
        return true;*/

        return cell != null && cell.canBeUsed && !cell.isFixed && gameManager.GetCellStatus(this, cell)
            && (cell.hour + initializer.activity.length < 24) && (initializer.activity.fullStomachLength > 0? gameManager.GetCellFoodStatus(this, cell) : true);
    }

public void OnMouseDown() {
        if(!FindFirstObjectByType<LevelManager>().paused) {
            if(!initializer.IsFixed()) {
                isHeld = true;
                AudioSource.PlayClipAtPoint(pickUp, gameObject.transform.position, audioVolume);
            }
            else {
                AudioSource.PlayClipAtPoint(fixedPickUp, gameObject.transform.position, audioVolume);
            }
        }
    }

    public void OnMouseUp() {
        if(!FindFirstObjectByType<LevelManager>().paused) {
            if(!initializer.IsFixed()) {
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
                }
                else {
                    isHeld = false;

                    ClaimCells();
                    if(isTouchingTrashCan) {
                        AudioSource.PlayClipAtPoint(trashSound, gameObject.transform.position, audioVolume);
                        gameManager.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true);
                        Destroy(gameObject.transform.parent.gameObject);
                    }
                    else {
                        AudioSource.PlayClipAtPoint(putDown, gameObject.transform.position, audioVolume);
                    }
                }
            }
        }
    }

    void Start() {
        gameManager = FindFirstObjectByType<LevelManager>();
    }

    public void ClaimCells() {
        LevelManager gameManager2 = FindFirstObjectByType<LevelManager>();

        if(occupiedCell != null) { gameManager2.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true); }
        occupiedCell = closestCell;
        gameManager2.FreeOrOccupyCells(this, closestCell.GetComponent<GridCell>(), false);
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
        if(isHeld && !initializer.IsFixed()) {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = -2f;
            gameObject.transform.position = targetPosition;
            //gameObject.transform.position = Vector3.Lerp(transform.position, worldPosition, 8f * Time.deltaTime);
        } else {
            targetPosition = new Vector3(initializer.shadowPanel.transform.position.x, initializer.shadowPanel.transform.position.y, -2f);
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, 5f * Time.deltaTime);
        }

        if(closestCell == null) {
            initializer.shadowPanel.GetComponent<ShadowPanel>().targetPosition = gameObject.transform.position;
        }
        else {
            initializer.shadowPanel.GetComponent<ShadowPanel>().targetPosition = new Vector3(closestCell.transform.position.x, closestCell.transform.position.y, closestCell.transform.position.z - 0.5f);
        }
    }
}