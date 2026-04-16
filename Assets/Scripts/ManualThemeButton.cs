using UnityEngine;
using TMPro;

public class ThemeManualButton : MonoBehaviour
{
    // Drag the specific MenuTheme asset (e.g., Jungle) into this slot in the Inspector
    [SerializeField] private MenuTheme myTheme; 
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Start()
    {
        // Set the button's text to match the theme name automatically
        if (myTheme != null && buttonText != null)
        {
            buttonText.text = myTheme.name;
        }
    }

    public void OnClickSelect()
    {
        if (myTheme != null)
        {
            // Tell the manager to switch to THIS specific theme
            GlobalGameManager.Instance.SetThemeManual(myTheme);
            Debug.Log("Switched to: " + myTheme.name);
        }
    }
}