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
    [SerializeField] private GameObject extendedShadowPanel;

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
        extendedPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        extendedShadowPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 0.7f);
        //textObject.GetComponent<TextMeshProUGUI>().color = new Color(activityColor.r * 0.35f, activityColor.g * 0.35f, activityColor.b * 0.35f, 1.0f);   //moved to theme system
        textObject.GetComponent<TextMeshProUGUI>().text = "    " + activity.title;


        //Sizing

        Vector2 panelSize = new Vector2(cellWidth, (cellHeight * activity.length) + (cellSpacing * (activity.length - 1)));
        yOffset = (cellHeight / 2f) - (panelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);
        mainPanel.GetComponent<Activity>().yOffset = yOffset;

        extendedPanel.GetComponent<SpriteRenderer>().size = panelSize;
        //extendedPanel.GetComponent<BoxCollider2D>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().offset = new Vector2(0, yOffset);
        extendedShadowPanel.GetComponent<SpriteRenderer>().size = panelSize;
    }

    // Update is called once per frame
    void Update()
    {
        extendedPanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.2f);
        extendedShadowPanel.transform.position = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y + yOffset, shadowPanel.transform.position.z + 0.2f);
        //extendedShadowPanel.GetComponent<ShadowPanel>().targetPosition = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y + yOffset, shadowPanel.transform.position.z + 0.2f);
    }
}
