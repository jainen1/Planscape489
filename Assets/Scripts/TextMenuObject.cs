using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;
    private float characterSpacing;

    [SerializeField] TextType textType = TextType.Basic;

    void OnEnable() { GlobalGameManager.OnUpdateThemeText += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateThemeText -= UpdateMenuObject; }

    private void Awake() {
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
        characterSpacing = gameObject.GetComponent <TextMeshProUGUI>().characterSpacing;
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
        float characterSpacingScale;

        switch(textType) {
            case TextType.Basic: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
                characterSpacingScale = menuTheme.mainCharacterSpacingScale;
                break;
            }
            case TextType.Timer: {
                font = menuTheme.timerFont;
                fontSizeScale = menuTheme.timerFontSizeScale;
                characterSpacingScale = menuTheme.timerCharacterSpacingScale;
                break;
            }
            default: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
                characterSpacingScale = menuTheme.mainCharacterSpacingScale;
                break;
            }
        }

        textComponent.font = font;
        textComponent.fontSize = fontSize * fontSizeScale;
        textComponent.characterSpacing = characterSpacing * characterSpacingScale;
    }
}

public enum TextType {
    Basic,
    Timer
}