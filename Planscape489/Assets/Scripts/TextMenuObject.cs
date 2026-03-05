using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;

    [SerializeField] TextType textType = TextType.Basic;

    void OnEnable() { GlobalGameManager.OnUpdateThemeText += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateThemeText -= UpdateMenuObject; }

    private void Awake() {
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
    }

    public void UpdateMenuObject() {
        MenuTheme menuTheme = GlobalGameManager.Instance.GetMenuTheme();
        TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();

        Color color = Color.red;

        bool brightOrDark;
        MenuObject menuObject = backgroundObject.GetComponent<MenuObject>();

        if(menuObject != null) { brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<MenuObject>().color, threshold); }
        else { brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<SpriteRenderer>().color, threshold); }

        color = brightOrDark ? menuTheme.brightTextColor : menuTheme.darkTextColor;
        textComponent.color = color;

        TMP_FontAsset font;
        float fontSizeScale;

        switch(textType) {
            case TextType.Basic: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
                break;
            }
            case TextType.Timer: {
                font = menuTheme.timerFont;
                fontSizeScale = menuTheme.timerFontSizeScale;
                break;
            }
            default: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
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