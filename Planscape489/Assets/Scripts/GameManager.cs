using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MenuTheme menuTheme;

    public GridCell[] cells;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTheme() {

    }

    public bool GetCellStatus(Activity activity, GridCell startCell) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = startCellIndex + activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            if(cells[i].occupyingActivity != null && cells[i].occupyingActivity != activity) {
                return false;
            }
        } return true;
    }

    public void OccupyCells(Activity activity, GridCell startCell) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = startCellIndex + activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            cells[i].occupyingActivity = activity;
        }
    }

    public void FreeCells(Activity activity, GridCell startCell) {
        int startCellIndex = GetGridCellIndex(startCell.day, startCell.hour);
        int endCellIndex = startCellIndex + activity.length - 1;
        for(int i = startCellIndex; i <= endCellIndex; i++) {
            cells[i].occupyingActivity = null;
        }
    }

    public int GetGridCellIndex(int day, int hour) {
        return ((day - 1) * 17) + (hour - 6);
        //Day 1, Cell 6 (index 0): ((1-1)*17)+(6-6) = 0
        //Day 2, Cell 21 (index 32): ((2-1)*17)+(21-6) = 32
        //Day 7, Cell 22 (index 118): ((7-1)*17)+(22-6) = 118
    }

    public GridCell GridCellFromIndex(int day, int hour) {
        return cells[GetGridCellIndex(day, hour)];
    }
}
