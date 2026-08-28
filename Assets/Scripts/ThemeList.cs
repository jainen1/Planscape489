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

        // 2. Gather loaded themes
        for(int i = 0; i < GlobalGameManager.GetLoadedThemes().Count; i++) {
            MenuTheme theme = GlobalGameManager.GetLoadedThemes()[i];
            // 3. Spawn a new button for every theme in the list
            GameObject newButton = Instantiate(itemPrefab, transform);
            itemList.Add(newButton);

            // 4. Access the script on the button to set the text/icon
            ThemeListItem themeListItem = newButton.GetComponent<ThemeListItem>();
            if (themeListItem != null) {
                themeListItem.theme = theme;
                themeListItem.index = i;
                themeListItem.SetupText();
            }
            else { Debug.LogError("The prefab is missing the ThemeListItem script!"); }
        }

        // Scrollbox will auto-resize.
    }
}