using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeList : MonoBehaviour
{
    [Header("SetupText")]
    [SerializeField] private GameObject itemPrefab; //  button template
    private List<GameObject> itemList = new List<GameObject>();
    private int index = 0;

    private void Awake() {
        CreateList(new List<MenuTheme>(GlobalGameManager.Instance.GetActiveMenuThemes()));
    }

    public void CreateList(List<MenuTheme> themes) 
    {
        // 1. Clear out any old buttons
        foreach (GameObject item in itemList) { Destroy(item); }
        itemList.Clear();

        // 2. Spawn a new button for every theme in your list
        foreach (MenuTheme theme in themes) { AddThemeButton(); index++; }

        // will auto-resize.
    }

    private void AddThemeButton() {
        // Create the button
        GameObject newButton = Instantiate(itemPrefab, transform);
        itemList.Add(newButton);

        // Access the script on the button to set the text/icon
        ThemeListItem script = newButton.GetComponent<ThemeListItem>();
        if (script != null) {
            script.SetThemeIndex(index);
            script.SetupText();
        } else {
            Debug.LogError("The firstItem is missing the ThemeListItem script!");
        }
    }
}