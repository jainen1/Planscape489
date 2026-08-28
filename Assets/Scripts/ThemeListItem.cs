using UnityEngine;
using TMPro;

public class ThemeListItem : MonoBehaviour
{
    public MenuTheme theme;
    public int index;
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetupText () {
        buttonText.text = theme.name;
    }

    public void OnClickSelect() {
        GlobalGameManager.SetThemeByIndex(index);
    }
}