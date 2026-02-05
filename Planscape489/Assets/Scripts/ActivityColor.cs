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

    [SerializeField] private bool isFixed;

    private void Awake() {
        ActivityGrid gridComponent = FindFirstObjectByType<ActivityGrid>();
        //Grid gridComponent = grid.GetComponent<Grid>();

        Vector2 newSize = new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * activity.length);

        gameObject.GetComponent<RectTransform>().SetSize(newSize);
        panel.GetComponent<BoxCollider2D>().size = newSize;
        textObject.GetComponent<RectTransform>().SetSize(newSize);

        //panel.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));
        //panelShadow.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));

        panel.GetComponent<Image>().color = new Color(activity.color.r, activity.color.g, activity.color.b, 1.0f);
        panelShadow.GetComponent<Image>().color = new Color(activity.color.r, activity.color.g, activity.color.b, 0.7f);
        textObject.GetComponent<TextMeshProUGUI>().color = new Color(activity.color.r * 0.35f, activity.color.g * 0.35f, activity.color.b * 0.35f, 1.0f);
        textObject.GetComponent<TextMeshProUGUI>().text = activity.title;

        lockIcon.SetActive(isFixed);
    }

    //temporary length randomizer
    private int RandomCellLength() {
        return Random.Range(1, 8);
    }
}
