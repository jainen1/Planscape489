using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    [SerializeField] public ActivityInitializer initializer;

    public bool isHeld = false;

    private List<GameObject> collidingCells = new List<GameObject>();
    private GameObject closestCell = null;
    private GameObject occupiedCell = null;

    [SerializeField] private bool isTouchingTrashCan = false;

    [SerializeField] private float mouseLerp = 100f;
    [SerializeField] private float targetLerp = 5f;

    [SerializeField] public TaskList.ActivityType activityType;

    void Update() {
        if(isHeld) {
            foreach(GameObject cell in collidingCells) {
                if(closestCell == null || Vector2.Distance(gameObject.transform.position, cell.transform.position) < Vector2.Distance(gameObject.transform.position, closestCell.transform.position)) {
                    closestCell = cell;
                }
            }
        }

        if(isHeld && !initializer.IsFixed()) {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = -2f;
            //gameObject.transform.position = targetPosition; //teleport to mouse position
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, mouseLerp * Time.deltaTime); //lerp to mouse position
        }
        else {
            Vector3 targetPosition = new Vector3(occupiedCell.transform.position.x, occupiedCell.transform.position.y, -2f);
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, targetLerp * Time.deltaTime);
        }

        if(closestCell != null && closestCell.GetComponent<GridCell>() != null && closestCell.GetComponent<GridCell>().isFixed && !initializer.IsFixed()) {
            collidingCells.Remove(closestCell);
            closestCell = null;
        }

        initializer.shadowPanel.transform.position = (closestCell == null || initializer.IsFixed()) ? gameObject.transform.position : new Vector3(closestCell.transform.position.x, closestCell.transform.position.y, closestCell.transform.position.z - 0.5f);
    }

    public void OnMouseDown() {
        if(FindFirstObjectByType<LevelManager>().levelIsActive) {
            AudioClip sfx = GlobalGameManager.GetCurrentMenuTheme().activityPickUpFail; //was originally 'fixedPickUp', but that has been merged with 'failedPickUp'.
            if(!initializer.IsFixed()) {
                isHeld = true;
                sfx = GlobalGameManager.GetCurrentMenuTheme().activityPickUp;
            }
            GlobalGameManager.PlayClip(sfx, "SFX Volume");
        }
    }

    public void OnMouseUp() {
        if(!initializer.IsFixed()) {
            if(closestCell == null) {
                foreach(GridCell cell in LevelManager.Instance.cells) {
                    if(CellIsAvailable(cell)) {
                        SetTargetCell(cell);
                        break;
                    }
                }
            }
            if(closestCell == null) {
                GlobalGameManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityPickUpFail, "SFX Volume");
                Destroy(gameObject.transform.parent.gameObject);
            }
            else {
                isHeld = false;

                ClaimCells();
                if(isTouchingTrashCan) {
                    GlobalGameManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityTrash, "SFX Volume");
                    LevelManager.Instance.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true);
                    LevelManager.Instance.ReturnTaskToList(initializer.activity);
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else {
                    GlobalGameManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityPutDown, "SFX Volume");
                }
            }
        }
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

    public void SetTargetCell(GridCell cell) {
        closestCell = cell.gameObject;
    }

    private bool CellIsAvailable(GridCell cell) {

        //Debug
        /*if(cell == null) { Debug.Log("Cell is null!"); return false; }
        if(!cell.canBeUsed) { Debug.Log("Cell cannot be used!"); return false; }
        if(cell.isFixed) { Debug.Log("Cell is fixed!"); return false; }
        if(!LevelManagerInstance.GetCellStatus(this, cell)) { Debug.Log("Cell status returned false!"); return false; }
        if(!(cell.hour + initializer.activity.length < 24)) { Debug.Log("Cell is beyond end of day!"); return false; }
        if(!(initializer.activity.fullStomachLength > 0 ? levelManager.GetCellFoodStatus(this, cell) : true)) { Debug.Log("Cell cannot be used for food!");  return false; }
        return true;*/

        return cell != null && cell.canBeUsed && !cell.isFixed && LevelManager.Instance.GetCellStatus(this, cell)
            && (cell.hour + initializer.activity.length < 24) && (initializer.activity.fullStomachLength > 0? LevelManager.Instance.GetCellFoodStatus(this, cell) : true);
    }

    public void ClaimCells() {
        if(occupiedCell != null) { LevelManager.Instance.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true); }
        occupiedCell = closestCell;
        LevelManager.Instance.FreeOrOccupyCells(this, closestCell.GetComponent<GridCell>(), false);
    }
}