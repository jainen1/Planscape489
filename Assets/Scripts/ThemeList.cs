using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeList", menuName = "Scriptable Objects/ThemeList")]
public class ThemeList : ScriptableObject {
    public List<MenuTheme> themes;

    //to see the list
    public void PrintThemes() 
    {
        for (int i = 0; i < themes.Count; i++) 
        {
            Debug.Log("Theme No. " + i + ": " + themes[i].name);
        }
    }

}