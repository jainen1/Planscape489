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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        Color activityColor = isFixed ? gameManager.menuTheme.fixedActivityColor : activity.color;

        mainPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        extendedPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 1.0f);
        shadowPanel.GetComponent<SpriteRenderer>().color = new Color(activityColor.r, activityColor.g, activityColor.b, 0.7f);
        //textObject.GetComponent<TextMeshProUGUI>().color = new Color(activityColor.r * 0.35f, activityColor.g * 0.35f, activityColor.b * 0.35f, 1.0f);   //moved to theme system
        textObject.GetComponent<TextMeshProUGUI>().text = activity.title;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
