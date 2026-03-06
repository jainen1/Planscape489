using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

<<<<<<< Updated upstream
    void OnEnable() { LevelManager.OnLateUpdateTheme += UpdateMenuObject; }
    void OnDisable() { LevelManager.OnLateUpdateTheme -= UpdateMenuObject; }
=======
    private float fontSize;

    [SerializeField] TextType textType = TextType.Basic;

    void OnEnable() { GlobalGameManager.OnUpdateThemeText += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateThemeText -= UpdateMenuObject; }
>>>>>>> Stashed changes

    private void Awake() {
<<<<<<< Updated upstream
=======
        fontSize = gameObject.GetComponent<TextMeshProUGUI>().fontSize;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<LevelManager>();
        Color color = Color.red;

        bool brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<MenuObject>().color, threshold);
        color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
        gameObject.GetComponent<TextMeshProUGUI>().color = color;
=======
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
>>>>>>> Stashed changes
    }
}