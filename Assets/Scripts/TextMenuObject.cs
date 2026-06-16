using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextMenuObject : MonoBehaviour
{
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    private float fontSize;
    private float characterSpacing;

    [SerializeField] TextType textType = TextType.Basic;

    void OnEnable() { GlobalGameManager.OnUpdateThemeText += OnThemeUpdate; }
    void OnDisable() { GlobalGameManager.OnUpdateThemeText -= OnThemeUpdate; }

    private void Awake() {
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
        characterSpacing = gameObject.GetComponent <TextMeshProUGUI>().characterSpacing;
    }

    public void OnThemeUpdate() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();
        TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();

        Color backgroundColor = Color.white;
        if(backgroundObject != null) {
            ReceivesThemeUpdates backgroundMenuObject = backgroundObject.GetComponent<ReceivesThemeUpdates>();
            SpriteRenderer backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();
            Image backgroundImage = backgroundObject.GetComponent<Image>();

            if(backgroundMenuObject != null) { backgroundColor = backgroundMenuObject.GetMainColor(); }
            else if(backgroundSpriteRenderer != null) { backgroundColor = backgroundSpriteRenderer.color; }
            else if(backgroundImage != null) { backgroundColor = backgroundImage.color; }
        }
        
        textComponent.color = SimpleMenuObject.GetBrightOrDarkColor(backgroundColor, threshold) ? menuTheme.brightTextColor : menuTheme.darkTextColor;

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