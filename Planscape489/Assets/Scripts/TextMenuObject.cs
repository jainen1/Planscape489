using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    private LevelManager gameManager;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;

    void OnEnable() { LevelManager.OnLateUpdateTheme += UpdateMenuObject; }
    void OnDisable() { LevelManager.OnLateUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<LevelManager>();
        TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();

        Color color = Color.red;
        bool brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<MenuObject>().color, threshold);
        color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
        textComponent.color = color;
        textComponent.font = gameManager.menuTheme.mainFont;
        textComponent.fontSize = fontSize * gameManager.menuTheme.mainFontSizeScale;
    }
}