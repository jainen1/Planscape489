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
            SoundManager.PlayClip(sfx, SoundManager.AudioChannels.sfx);
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
                SoundManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityPickUpFail, SoundManager.AudioChannels.sfx);
                Destroy(gameObject.transform.parent.gameObject);
            }
            else {
                isHeld = false;

                ClaimCells();
                if(isTouchingTrashCan) {
                    SoundManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityTrash, SoundManager.AudioChannels.sfx);
                    LevelManager.Instance.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true);
                    LevelManager.Instance.ReturnTaskToList(initializer.activity);
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else {
                    SoundManager.PlayClip(GlobalGameManager.GetCurrentMenuTheme().activityPutDown, SoundManager.AudioChannels.sfx);
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
        if(cell == null) { /*Debug.LogWarning("Cell unavailable: Cell is null!");*/ return false; }
        if(!cell.canBeUsed) { /*Debug.LogWarning("Cell unavailable: Cell cannot be used!");*/ return false; }
        if(cell.isFixed) { /*Debug.LogWarning("Cell unavailable: Cell is fixed!");*/ return false; }
        if(cell.hour + initializer.activity.length >= 24) { /*Debug.LogWarning("Cell unavailable: Cell is beyond end of day!");*/ return false; }

        //previously 'GetCellStatus' in LevelManager. Tests if all spaces that would be taken up by the activity are free
        int startCellIndex = LevelManager.GetGridCellIndex(cell.day, cell.hour);
        int endCellIndex = startCellIndex + initializer.activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            if(endCellIndex > LevelManager.Instance.cells.Length - 1 || (LevelManager.Instance.cells[i].occupyingActivity != null && LevelManager.Instance.cells[i].occupyingActivity != this)) {
                //Debug.LogWarning("Cell unavailable: Cell(s) occupied!");
                return false;
            }
        }

        //previously 'GetCellFoodStatus' in LevelManager. Tests if all spaces that would be taken up by the activity's 'full stomach' are free of 'full stomach'
        if(initializer.activity.fullStomachLength > 0) { 
            int foodEndCellIndex = Mathf.Min(startCellIndex + initializer.activity.fullStomachLength - 1, LevelManager.GetGridCellIndex(cell.day, 22));
            for(int i = startCellIndex; i <= foodEndCellIndex; i++) {
                if(foodEndCellIndex > LevelManager.Instance.cells.Length - 1 || (LevelManager.Instance.cells[i].occupyingFoodActivity != null && LevelManager.Instance.cells[i].occupyingFoodActivity != this)) {
                    //Debug.Log("Cell unavailable: Activity is food, and cell is occupied by food!");
                    return false;
                }
            }
        }
        return true;
    }

    public void ClaimCells() {
        if(occupiedCell != null) { LevelManager.Instance.FreeOrOccupyCells(this, occupiedCell.GetComponent<GridCell>(), true); }
        occupiedCell = closestCell;
        LevelManager.Instance.FreeOrOccupyCells(this, closestCell.GetComponent<GridCell>(), false);
    }
}