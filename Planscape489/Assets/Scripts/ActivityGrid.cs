using UnityEngine;

public class ActivityGrid : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [SerializeField] private BoxCollider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collider = gameObject.GetComponent<BoxCollider2D>();


        //gameObject.GetComponent<RectTransform>().rect.yMax = 0;
        Debug.Log("Cell width = " + getCellWidth() + ", Cell height = " + getCellHeight());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getCellWidth() {
        return collider.size.x / columns;
    }

    public float getCellHeight() {
        return collider.size.y / rows;
    }
}
