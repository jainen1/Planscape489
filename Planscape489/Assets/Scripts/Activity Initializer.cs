using TMPro;
using UnityEngine;

public class ActivityInitializer : MonoBehaviour
{
    [HideInInspector] public ActivityObject activity;

    [Header("Panels")]
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject extendedPanel;
    [SerializeField] public GameObject fullStomachPanel;
    [SerializeField] public GameObject fixedBorder;
    [SerializeField] public GameObject shadowPanel;
    [SerializeField] public GameObject extendedShadowPanel;
    [SerializeField] public GameObject title;
    [SerializeField] public GameObject happinessText;
    [SerializeField] public GameObject moneyText;

    [SerializeField] private bool isFixed;

    [Header("Cell Sizes")]
    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private float cellSpacing;

    private float yOffset;
    private float yOffsetStomach;

    public bool shouldDisplayFixedBorder = false;

    void Update()
    {
        extendedPanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.19f);
        fullStomachPanel.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffsetStomach, mainPanel.transform.position.z + 0.2f);
        fixedBorder.transform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y + yOffset, mainPanel.transform.position.z + 0.18f);
        extendedShadowPanel.transform.position = new Vector3(shadowPanel.transform.position.x, shadowPanel.transform.position.y + yOffset, shadowPanel.transform.position.z + 0.2f);
    }

    public void Initialize() {
        Vector2 panelSize = new Vector2(cellWidth, (cellHeight * activity.length) + (cellSpacing * (activity.length - 1)));
        Vector2 fullStomachPanelSize = new Vector2(cellWidth, (cellHeight * activity.fullStomachLength) + (cellSpacing * (activity.fullStomachLength - 1)));
        yOffset = (cellHeight / 2f) - (panelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);
        yOffsetStomach = (cellHeight / 2f) - (fullStomachPanelSize.y / 2); //(originalSize / 2.0) - (bigSize / 2.0);

        if(activity.fullStomachLength > 0) {
            fullStomachPanel.SetActive(true);
        }

        extendedPanel.GetComponent<SpriteRenderer>().size = panelSize;
        fullStomachPanel.GetComponent<SpriteRenderer>().size = fullStomachPanelSize;
        extendedShadowPanel.GetComponent<SpriteRenderer>().size = panelSize;
        fixedBorder.GetComponent<SpriteRenderer>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().size = panelSize;
        mainPanel.GetComponent<BoxCollider2D>().offset = new Vector2(0, yOffset);

        title.GetComponent<TextMeshProUGUI>().text = activity.title;
        happinessText.GetComponent<TextMeshProUGUI>().text = (activity.happiness >= 0? "H+" : "H-") + Mathf.Abs(activity.happiness * activity.length);
        moneyText.GetComponent<TextMeshProUGUI>().text = (activity.money >= 0? "$+" : "$-") + Mathf.Abs(activity.money * activity.length);

        extendedPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fullStomachPanel.GetComponent<MenuObject>().UpdateMenuObject();
        extendedShadowPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fixedBorder.GetComponent<MenuObject>().UpdateMenuObject();
        title.GetComponent<TextMenuObject>().UpdateMenuObject();
        happinessText.GetComponent<MenuObject>().UpdateMenuObject();
        moneyText.GetComponent<MenuObject>().UpdateMenuObject();
    }
    public void SetFixed(bool x) {
        isFixed = x;
        fixedBorder.SetActive(x && shouldDisplayFixedBorder);
        extendedPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fullStomachPanel.GetComponent<MenuObject>().UpdateMenuObject();
        extendedShadowPanel.GetComponent<MenuObject>().UpdateMenuObject();
        fixedBorder.GetComponent<MenuObject>().UpdateMenuObject();
        title.GetComponent<TextMenuObject>().UpdateMenuObject();
        happinessText.GetComponent<MenuObject>().UpdateMenuObject();
        moneyText.GetComponent<MenuObject>().UpdateMenuObject();
    }

    public bool IsFixed() {
        return isFixed;
    }
}