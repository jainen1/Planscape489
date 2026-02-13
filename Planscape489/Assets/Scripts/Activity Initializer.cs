using TMPro;
using UnityEngine;

public class ActivityInitializer : MonoBehaviour
{
    public ActivityObject activity;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject extendedPanel;
    [SerializeField] private GameObject shadowPanel;
    [SerializeField] private GameObject extendedShadowPanel;

    [SerializeField] private GameObject textObject;

    public bool isFixed;

    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private float cellSpacing;

    private float yOffset;

    void Update()
    {
        extendedPanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.2f);
        extendedShadowPanel.transform.position = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y + yOffset, shadowPanel.transform.position.z + 0.2f);
    }

    public void Initialize() {
        Vector2 panelSize = new Vector2(cellWidth, (cellHeight * activity.length) + (cellSpacing * (activity.length - 1)));
        yOffset = (cellHeight / 2f) - (panelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);
        mainPanel.GetComponent<Activity>().yOffset = yOffset;
        mainPanel.GetComponent<Activity>().length = activity.length;

        extendedPanel.GetComponent<SpriteRenderer>().size = panelSize;
        extendedShadowPanel.GetComponent<SpriteRenderer>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().offset = new Vector2(0, yOffset);

        textObject.GetComponent<TextMeshProUGUI>().text = activity.title;
    }
}