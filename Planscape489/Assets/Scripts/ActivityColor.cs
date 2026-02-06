using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActivityColor : MonoBehaviour
{
    [SerializeField] private ActivityObject activity;

    [SerializeField] private GameObject grid;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panelShadow;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject textObject;

    [SerializeField] public bool isFixed;

    private void Awake() {
        ActivityGrid gridComponent = FindFirstObjectByType<ActivityGrid>();
        //Grid gridComponent = grid.GetComponent<Grid>();

        Vector2 newSize = new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * activity.length);

        gameObject.GetComponent<RectTransform>().SetSize(newSize);
        panel.GetComponent<BoxCollider2D>().size = newSize;
        textObject.GetComponent<RectTransform>().SetSize(newSize);

        //panel.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));
        //panelShadow.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));

        Color activityColor = isFixed ? Color.gray9 : activity.color;
        panel.GetComponent<Image>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        panelShadow.GetComponent<Image>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 0.7f);
        textObject.GetComponent<TextMeshProUGUI>().color = new Color(activityColor.r * 0.35f, activityColor.g * 0.35f, activityColor.b * 0.35f, 1.0f);
        textObject.GetComponent<TextMeshProUGUI>().text = activity.title;

        lockIcon.SetActive(isFixed);
        //lockIcon.GetComponent<RectTransform>().SetSize(Vector3.one * (1 - 0.2f * (5 - activity.length)));
        lockIcon.transform.localScale = Vector3.one * (1 - 0.2f * (5 - activity.length));
        panel.GetComponent<Activity>().SetFixed(isFixed);
    }

    //temporary length randomizer
    private int RandomCellLength() {
        return Random.Range(1, 8);
    }
}
