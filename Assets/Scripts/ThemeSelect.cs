using UnityEngine;

public class ThemeSelect : MonoBehaviour
{
    public ThemeList allThemes;

    void Start() {

        if (allThemes.themes.Count==0) {
            Debug.Log("zero themes");
        }
        else {
            Debug.Log("printing themes");
            allThemes.PrintThemes();
        }
        
    }

    public void SetThemeByIndex(int index)
    {
        if (index >= 0 && index < allThemes.themes.Count)
        {
            MenuTheme selected = allThemes.themes[index];
            ApplyTheme(selected);
        }
    }

    public void ApplyTheme(MenuTheme theme) {
    
       

       Debug.Log("applying theme" + theme.name);

        //switch to theme 


    }
}
