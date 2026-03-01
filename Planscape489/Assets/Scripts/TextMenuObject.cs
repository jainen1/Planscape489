using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    private LevelManager gameManager;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;

    [SerializeField] TextType textType = TextType.Basic;

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

        bool brightOrDark;
        MenuObject menuObject = backgroundObject.GetComponent<MenuObject>();

        if(menuObject != null) { brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<MenuObject>().color, threshold); }
        else { brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<SpriteRenderer>().color, threshold); }

        color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
        textComponent.color = color;

        TMP_FontAsset font;
        float fontSizeScale;

        switch(textType) {
            case TextType.Basic: {
                font = gameManager.menuTheme.mainFont;
                fontSizeScale = gameManager.menuTheme.mainFontSizeScale;
                break;
            }
            case TextType.Timer: {
                font = gameManager.menuTheme.timerFont;
                fontSizeScale = gameManager.menuTheme.timerFontSizeScale;
                break;
            }
            default: {
                font = gameManager.menuTheme.mainFont;
                fontSizeScale = gameManager.menuTheme.mainFontSizeScale;
                break;
            }
        }

        textComponent.font = font;
        textComponent.fontSize = fontSize * fontSizeScale;
    }
}

public enum TextType {
    Basic,
    Timer
}