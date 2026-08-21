using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ThemeList : MonoBehaviour
{
    [Header("SetupText")]
    [SerializeField] private GameObject itemPrefab; //  button template
    [SerializeField] private List<GameObject> itemList = new List<GameObject>();

    private void Awake() {
        // 1. Clear out any old buttons
        foreach (GameObject item in itemList) { Destroy(item); }
        itemList.Clear();

        // 2. Gather JSON files
        List<MenuTheme> themes = new List<MenuTheme>(GlobalGameManager.GetActiveMenuThemes());
        foreach (MenuTheme theme in themes) {
            // 3. Spawn a new button for every theme in your list

            // Create the button
            GameObject newButton = Instantiate(itemPrefab, transform);
            itemList.Add(newButton);

            // Access the script on the button to set the text/icon
            ThemeListItem script = newButton.GetComponent<ThemeListItem>();
            if (script != null) { script.theme = theme; }
            else { Debug.LogError("The prefab is missing the ThemeListItem script!"); }
        }

        // Scrollbox will auto-resize.
    }
}