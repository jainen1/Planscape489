using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ActivityInitializer : MonoBehaviour
{
    [SerializeField] ActivityObject activity;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject extendedPanel;
    [SerializeField] private GameObject shadowPanel;

    [SerializeField] private GameObject textObject;

    public bool isFixed;

    private GameManager gameManager;

    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private float cellSpacing;

    private float yOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        //Coloring

        Color activityColor = isFixed ? gameManager.menuTheme.fixedActivityColor : activity.color;
        mainPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        extendedPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        shadowPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 0.7f);
        //textObject.GetComponent<TextMeshProUGUI>().color = new Color(activityColor.r * 0.35f, activityColor.g * 0.35f, activityColor.b * 0.35f, 1.0f);   //moved to theme system
        textObject.GetComponent<TextMeshProUGUI>().text = "    " + activity.title;


        //Sizing

        mainPanel.GetComponent<SpriteRenderer>().size = new Vector2(cellWidth, cellHeight);

        Vector2 panelSize = new Vector2(cellWidth, (cellHeight * activity.length) + (cellSpacing * (activity.length - 1)));
        extendedPanel.GetComponent<SpriteRenderer>().size = panelSize;
        extendedPanel.GetComponent<BoxCollider2D>().size = panelSize;
        shadowPanel.GetComponent<SpriteRenderer>().size = panelSize;
        yOffset = (cellHeight / 2f) - (panelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offsetPosition = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z);
        extendedPanel.transform.position = offsetPosition;
        shadowPanel.GetComponent<ShadowPanel>().targetPosition = offsetPosition;
    }
}
