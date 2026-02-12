using UnityEngine;
using UnityEngine.Rendering;

public class GridCell : MonoBehaviour
{
    public bool canBeUsed;

    public int day;
    public int hour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(canBeUsed) {
            hour = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
            day = int.Parse(gameObject.transform.parent.name.Substring(gameObject.transform.parent.name.Length - 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
