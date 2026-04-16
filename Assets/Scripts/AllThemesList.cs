using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllThemesList : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject MenuThemePrefab; //  button template
    [SerializeField] private ThemeList themeListAsset;   

    private List<GameObject> spawnedButtons = new List<GameObject>();

    private void Awake() 
    {
        if (themeListAsset != null)
        {
            CreateList(themeListAsset.themes);
        }
        else
        {
            Debug.LogError("Please assign the ThemeList Asset in the Inspector!");
        }
    }

    public void CreateList(List<MenuTheme> themes) 
    {
        // 1. Clear out any old buttons
        foreach (GameObject item in spawnedButtons)
        {
            Destroy(item);
        }
        spawnedButtons.Clear();

        // 2. Spawn a new button for every theme in your list
        foreach (MenuTheme theme in themes)
        {
            AddThemeButton(theme);
        }

        // will auto-resize.
    }

    private void AddThemeButton(MenuTheme themeData)
    {
        // Create the button
        GameObject newButton = Instantiate(MenuThemePrefab, transform);
        spawnedButtons.Add(newButton);

        // Access the script on the button to set the text/icon
        ThemeItem script = newButton.GetComponent<ThemeItem>();
        if (script != null)
        {
            script.Setup(themeData);
        }
        else {
            Debug.LogError("The MenuThemePrefab is missing the ThemeItem script!");
        }
    }
}