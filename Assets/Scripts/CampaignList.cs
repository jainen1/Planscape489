using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignList : MonoBehaviour
{
    [Header("SetupText")]
    [SerializeField] private GameObject itemPrefab; //  button template
    private List<GameObject> itemList = new List<GameObject>();

    private void Awake() {
        CreateList(new List<Campaign>(Resources.LoadAll<Campaign>("Campaigns")));
    }

    public void CreateList(List<Campaign> campaigns) 
    {
        // 1. Clear out any old buttons
        foreach (GameObject item in itemList) { Destroy(item); }
        itemList.Clear();

        // 2. Spawn a new button for every theme in your list
        foreach (Campaign campaign in campaigns) { AddCampaignContainer(campaign); }

        // will auto-resize.
    }

    private void AddCampaignContainer(Campaign campaign) {
        // Create the button
        GameObject newButton = Instantiate(itemPrefab, transform);
        itemList.Add(newButton);

        // Access the script on the button to set the text/icon
        CampaignListItem script = newButton.GetComponent<CampaignListItem>();
        if (script != null) {
            script.campaign = campaign;
            script.SetupText();
        } else { Debug.LogError("The prefab is missing the CampaignListItem script!"); }
    }
}