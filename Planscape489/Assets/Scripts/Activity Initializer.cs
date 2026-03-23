using TMPro;
using UnityEngine;

public class ActivityInitializer : MonoBehaviour
{
    [HideInInspector] public ActivityObject activity;

    [Header("Panels")]
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject visiblePanel;
    [SerializeField] public GameObject fullStomachPanel;
    [SerializeField] public GameObject fixedBorder;
    [SerializeField] public GameObject shadowPanel;
    [SerializeField] public GameObject visibleShadowPanel;
    [SerializeField] public GameObject title;
    [SerializeField] public GameObject resourceTextComponent;

    [SerializeField] private bool isFixed;

    [Header("Cell Sizes")]
    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private float cellSpacing;

    private float yOffset;
    private float yOffsetStomach;

    public bool displayFixedBorder = false;

    void Update()
    {
        visiblePanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.19f);
        fullStomachPanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffsetStomach, mainPanel.transform.position.z + 0.2f);
        fixedBorder.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.18f);
        visibleShadowPanel.transform.position = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y + yOffset, shadowPanel.transform.position.z + 0.2f);
    }

    public void Initialize() {
        Vector2 panelSize = new Vector2(cellWidth, (cellHeight * activity.length) + (cellSpacing * (activity.length - 1)));
        Vector2 fullStomachPanelSize = new Vector2(cellWidth, (cellHeight * activity.fullStomachLength) + (cellSpacing * (activity.fullStomachLength - 1)));
        yOffset = (cellHeight / 2f) - (panelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);
        yOffsetStomach = (cellHeight / 2f) - (fullStomachPanelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);

        if(activity.fullStomachLength > 0) {
            fullStomachPanel.SetActive(true);
        }

        visiblePanel.GetComponent<SpriteRenderer>().size = panelSize;
        fullStomachPanel.GetComponent<SpriteRenderer>().size = fullStomachPanelSize;
        visibleShadowPanel.GetComponent<SpriteRenderer>().size = panelSize;
        fixedBorder.GetComponent<SpriteRenderer>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().offset = new Vector2(0, yOffset);

        title.GetComponent<TextMeshProUGUI>().text = activity.title;


        string resourceText = string.Empty;
        if(activity.happiness != 0) { resourceText += (activity.happiness >= 0 ? "H+" : "H-") + Mathf.Abs(activity.happiness * activity.length); }
        if(activity.happiness != 0) { resourceText += "\n" + (activity.money >= 0 ? "$+" : "$-") + Mathf.Abs(activity.money * activity.length); }
        resourceTextComponent.GetComponent<TextMeshProUGUI>().text = resourceText;

        visiblePanel.GetComponent<MenuObject>().UpdateMenuObject();
        fullStomachPanel.GetComponent<MenuObject>().UpdateMenuObject();
        visibleShadowPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fixedBorder.GetComponent<MenuObject>().UpdateMenuObject();
        title.GetComponent<TextMenuObject>().UpdateMenuObject();
        resourceTextComponent.GetComponent<MenuObject>().UpdateMenuObject();
    }
    public void SetFixed(bool x) {
        isFixed = x;
        fixedBorder.SetActive(x && displayFixedBorder);
        visiblePanel.GetComponent<MenuObject>().UpdateMenuObject();
        fullStomachPanel.GetComponent<MenuObject>().UpdateMenuObject();
        visibleShadowPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fixedBorder.GetComponent<MenuObject>().UpdateMenuObject();
        title.GetComponent<TextMenuObject>().UpdateMenuObject();
        resourceTextComponent.GetComponent<MenuObject>().UpdateMenuObject();
    }

    public bool IsFixed() {
        return isFixed;
    }
}