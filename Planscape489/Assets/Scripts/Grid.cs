using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    private BoxCollider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();


        //gameObject.GetComponent<RectTransform>().rect.yMax = 0;
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
