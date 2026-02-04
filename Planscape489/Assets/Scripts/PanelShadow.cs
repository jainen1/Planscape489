using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShadow : MonoBehaviour
{
    public float gridSizeX = 29f;
    public float gridSizeY = 255f;

    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    public GameObject parentActivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPoint = parentActivity.transform.position;
        transform.position = new Vector3(
            RoundToNearestGridPoint(targetPoint.x, gridSizeX) + offsetX,
            RoundToNearestGridPoint(targetPoint.y, gridSizeY) + offsetY,
            transform.position.z);
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
