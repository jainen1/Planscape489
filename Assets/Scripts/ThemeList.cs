using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeList", menuName = "Scriptable Objects/ThemeList")]
public class ThemeList : ScriptableObject {
    public List<MenuTheme> themes;
}