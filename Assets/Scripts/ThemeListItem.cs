using UnityEngine;
using TMPro;

public class ThemeListItem : MonoBehaviour
{
    public MenuTheme theme;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start() {
        if(buttonText != null) {
            buttonText.text = theme.name;
        }
    }

    public void OnClickSelect() {
        GlobalGameManager.SetThemeManually(theme);
    }
}