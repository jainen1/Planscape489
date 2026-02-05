using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActivityColor : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private Color panelColor;

    [SerializeField] private GameObject grid;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panelShadow;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject textObject;

    [SerializeField] private bool isFixed;

    private void Awake() {
        ActivityGrid gridComponent = FindFirstObjectByType<ActivityGrid>();
        //Grid gridComponent = grid.GetComponent<Grid>();
        int cellLength = 5;
        Debug.Log("Old size is " + panel.GetComponent<RectTransform>().GetSize());
        panel.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));
        panelShadow.GetComponent<RectTransform>().SetSize(new Vector2(gridComponent.getCellWidth(), gridComponent.getCellHeight() * cellLength));
        Debug.Log("New size is " + panel.GetComponent<RectTransform>().GetSize());

        panel.GetComponent<Image>().color = panelColor;
        panelShadow.GetComponent<Image>().color = new Color(panelColor.r, panelColor.g, panelColor.b, 0.7f);
        textObject.GetComponent<TextMeshProUGUI>().color = new Color(panelColor.r * 0.35f, panelColor.g * 0.35f, panelColor.b * 0.35f, panelColor.a);
        textObject.GetComponent<TextMeshProUGUI>().text = title;

        lockIcon.SetActive(isFixed);
    }
}
