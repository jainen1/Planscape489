using UnityEngine;
using TMPro;

public class ThemeItem : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI themeNameText;
   private MenuTheme themeData;

   public void Setup(MenuTheme theme)
   {
       themeData = theme;
       if (themeNameText != null)
       {
           themeNameText.text = theme.name; 
       }
   }
}
