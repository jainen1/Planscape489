using TMPro;
using UnityEngine;

public class TextMenuObject : MonoBehaviour
{
    private LevelManager gameManager;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private int threshold = 128;

    void OnEnable() { LevelManager.OnLateUpdateTheme += UpdateMenuObject; }
    void OnDisable() { LevelManager.OnLateUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        gameManager = FindFirstObjectByType<LevelManager>();
        Color color = Color.red;

        bool brightOrDark = MenuObject.GetBrightOrDarkColor(backgroundObject.GetComponent<MenuObject>().color, threshold);
        color = brightOrDark ? gameManager.menuTheme.brightTextColor : gameManager.menuTheme.darkTextColor;
        gameObject.GetComponent<TextMeshProUGUI>().color = color;
    }
}