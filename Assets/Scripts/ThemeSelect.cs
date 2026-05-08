using UnityEngine;

public class ThemeSelect : MonoBehaviour
{
    void Start() {
        MenuTheme[] themes = GlobalGameManager.Instance.GetThemeList();
        if (themes.Length==0) {
            Debug.Log("zero themes");
        } else {
            Debug.Log("printing themes");
            for(int i = 0; i < themes.Length; i++) {
                Debug.Log("Theme No. " + i + ": " + themes[i].name);
            }
        }
    }

    public void SetThemeByIndex(int index) {
        GlobalGameManager.Instance.SetThemeByIndex(index);
    }

    public void ApplyTheme(MenuTheme theme) {
        Debug.Log("applying theme" + theme.name);
        //switch to theme
    }
}