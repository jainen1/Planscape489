using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CampaignList : MonoBehaviour
{
    [Header("SetupText")]
    [SerializeField] private GameObject itemPrefab; //  button template
    [SerializeField] private List<GameObject> itemList = new List<GameObject>();

    private void Awake() {
        // 1. Clear out any old buttons
        foreach (GameObject item in itemList) { Destroy(item); }
        itemList.Clear();

        // 2. Gather JSON files
        foreach(string file in JExtraUtility.LoadJsonFilesOfType("Campaigns")) {
            // 3. Convert JSON files
            Campaign campaign = JsonUtility.FromJson<Campaign.JData>(File.ReadAllText(file)).LoadJData();

            // 4. Spawn a new button for every campaign in the list
            GameObject newButton = Instantiate(itemPrefab, transform);
            itemList.Add(newButton);

            // 5. Access the script on the button to set the text/icon
            CampaignListItem campaignListItem = newButton.GetComponent<CampaignListItem>();
            if (campaignListItem != null) {
                campaignListItem.campaign = campaign;
                campaignListItem.SetupText();
            } 
            else { Debug.LogError("The prefab is missing the CampaignListItem script!"); }
        }

        // Scrollbox will auto-resize.
    }
}