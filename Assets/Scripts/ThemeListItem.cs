using UnityEngine;
using TMPro;

public class ThemeListItem : MonoBehaviour
{
    [SerializeField] private int themeIndex;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start() {
        // Set the button's text to match the theme name automatically
        SetupText();
    }

    public int GetThemeIndex() { return themeIndex; }
    public void SetThemeIndex(int index) { themeIndex = index; }

    public void OnClickSelect() {
        GlobalGameManager.SetThemeByIndex(themeIndex);
        //Debug.Log("Switched to: " + GlobalGameManager.Instance.GetActiveMenuThemes()[themeIndex].name);
    }

    public void SetupText() {
        if(buttonText != null) {
            buttonText.text = GlobalGameManager.GetActiveMenuThemes()[themeIndex].name;
        }
    }
}