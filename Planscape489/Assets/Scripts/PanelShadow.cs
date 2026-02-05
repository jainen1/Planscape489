using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShadow : MonoBehaviour
{

    private ActivityGrid gridComponent;

    public GameObject parentActivity;

    // Start is called before the first frame update
    void Start()
    {
        gridComponent = FindFirstObjectByType<ActivityGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPoint = parentActivity.transform.position;
        /*transform.position = new Vector3(
            RoundToNearestGridPoint(targetPoint.x, gridSizeX) + offsetX,
            RoundToNearestGridPoint(targetPoint.y, gridSizeY) + offsetY,
            transform.position.z);*/

        Vector3[] v = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(v);

        Vector3 topLeftmostPosition = v[1];
        Vector3 posDifference = transform.position - topLeftmostPosition;

        Vector3[] v2 = new Vector3[4];

        gridComponent.gameObject.GetComponent<RectTransform>().GetWorldCorners(v2);

        Vector3 cornerDifference = v[1] - v2[1];

        /*transform.position = new Vector3(
            RoundToNearestGridPoint(targetPoint.x, gridComponent.getCellWidth()) + gridComponent.offsetX,
            RoundToNearestGridPoint(targetPoint.y, gridComponent.getCellHeight()) + gridComponent.offsetY,
            transform.position.z);*/

        /*transform.position = new Vector3(
            Mathf.Clamp(RoundToNearestGridPoint2(targetPoint.x, gridComponent.getCellWidth()) + gridComponent.offsetX, v2[1].x, v2[1].x + gridComponent.getCellWidth() * gridComponent.rows),
            Mathf.Clamp(RoundToNearestGridPoint2(targetPoint.y, gridComponent.getCellHeight()) + gridComponent.offsetY, v2[1].y, v2[1].y + gridComponent.getCellHeight() * gridComponent.columns),
            transform.position.z);*/

        /*transform.position = new Vector3(
            Mathf.Clamp(RoundToNearestGridPoint2(targetPoint.x, gridComponent.getCellWidth()) + gridComponent.offsetX, v2[1].x, v2[2].x),
            Mathf.Clamp(RoundToNearestGridPoint2(targetPoint.y, gridComponent.getCellHeight()) + gridComponent.offsetY, v2[0].y, v2[1].y),
            transform.position.z);*/

        transform.position = new Vector3(
            RoundToNearestGridPoint2(Mathf.Clamp(targetPoint.x, v2[1].x, v2[2].x), gridComponent.getCellWidth()) + gridComponent.offsetX,
            RoundToNearestGridPoint2(Mathf.Clamp(targetPoint.y, v2[0].y, v2[1].y), gridComponent.getCellHeight()) + gridComponent.offsetY,
            transform.position.z);

        Vector2 cell = new Vector2(transform.position.x / gridComponent.getCellWidth(), transform.position.y / gridComponent.getCellHeight());
    }

    float RoundToNearestGridPoint2(float pos, float gridSize) {
        float xDiff = pos % gridSize;
        bool isPositive = pos > 0 ? true : false;
        pos -= xDiff;
        if(Mathf.Abs(xDiff) > (gridSize / 2)) {
            if(isPositive) {
                pos += gridSize;
            }
            else {
                pos -= gridSize;
            }
        }
        return pos;
    }

    float RoundToNearestGridPoint(float pos, float gridSize) {
        float xDiff = pos % gridSize;
        bool isPositive = pos > 0 ? true : false;
        pos -= xDiff;
        if(Mathf.Abs(xDiff) > (gridSize / 2)) {
            if(isPositive) {
                pos += gridSize;
            }
            else {
                pos -= gridSize;
            }
        }
        return pos;
    }
}
