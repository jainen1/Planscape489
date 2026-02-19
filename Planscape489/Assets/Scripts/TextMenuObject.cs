using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    void OnEnable() { GameManager.OnLateUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GameManager.OnLateUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<GameManager>();
        Color color = Color.red;

        bool brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<SpriteRenderer>().color, threshold);
        color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
        gameObject.GetComponent<TextMeshProUGUI>().color = color;
    }
}