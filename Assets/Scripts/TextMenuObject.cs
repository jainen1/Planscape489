using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextMenuObject : MonoBehaviour, ReceivesThemeUpdates
{
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;
    private float characterSpacing;
    private float lineSpacing;

    [SerializeField] TextType textType = TextType.Basic;

    void OnEnable() { GlobalGameManager.OnUpdateThemeText += OnThemeUpdate; }
    void OnDisable() { GlobalGameManager.OnUpdateThemeText -= OnThemeUpdate; }

    private void Awake() {
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
        characterSpacing = gameObject.GetComponent<TextMeshProUGUI>().characterSpacing;
        lineSpacing = gameObject.GetComponent<TextMeshProUGUI>().lineSpacing;
    }

    public Color GetMainColor () {
        Color backgroundColor = Color.white;
        if(backgroundObject != null) {
            ReceivesThemeUpdates backgroundMenuObject = backgroundObject.GetComponent<ReceivesThemeUpdates>();
            SpriteRenderer backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();
            Image backgroundImage = backgroundObject.GetComponent<Image>();

            if(backgroundMenuObject != null) { backgroundColor = backgroundMenuObject.GetMainColor(); }
            else if(backgroundSpriteRenderer != null) { backgroundColor = backgroundSpriteRenderer.color; }
            else if(backgroundImage != null) { backgroundColor = backgroundImage.color; }
        }

        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();
        return SimpleMenuObject.GetBrightOrDarkColor(backgroundColor, threshold) ? menuTheme.brightTextColor : menuTheme.darkTextColor;
    }

    public void OnThemeUpdate() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();
        TMP_FontAsset font;
        float fontSizeScale;
        float characterSpacingScale;
        float lineSpacingScale;

        switch(textType) {
            case TextType.Basic: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
                characterSpacingScale = menuTheme.mainCharacterSpacingScale;
                lineSpacingScale = menuTheme.mainLineSpacingScale;
                break;
            }
            case TextType.Timer: {
                font = menuTheme.timerFont;
                fontSizeScale = menuTheme.timerFontSizeScale;
                characterSpacingScale = menuTheme.timerCharacterSpacingScale;
                lineSpacingScale = menuTheme.timerLineSpacingScale;
                break;
            }
            default: {
                font = menuTheme.mainFont;
                fontSizeScale = menuTheme.mainFontSizeScale;
                characterSpacingScale = menuTheme.mainCharacterSpacingScale;
                lineSpacingScale = menuTheme.mainLineSpacingScale;
                break;
            }
        }

        TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        textComponent.color = GetMainColor();
        textComponent.font = font;
        textComponent.fontSize = fontSize * fontSizeScale;
        textComponent.characterSpacing = characterSpacing * characterSpacingScale;
        textComponent.lineSpacing = lineSpacing * characterSpacingScale;
    }
}

public enum TextType {
    Basic,
    Timer
}