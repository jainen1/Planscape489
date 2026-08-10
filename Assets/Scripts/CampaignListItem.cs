using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampaignListItem : MonoBehaviour
{
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    public Campaign campaign;

    private void Start() {
        // Set the button's text to match the theme name automatically
        SetupText();
    }

    public void OnClickSelect() {
        GlobalGameManager.SetCampaignAndPlay(campaign);
        //Debug.Log("Switched to: " + GlobalGameManager.Instance.GetActiveMenuThemes()[themeIndex].name);
    }

    public void SetupText() {
        if(title != null) {
            thumbnail.sprite = campaign.thumbnail;
            title.text = campaign.name;
            description.text = campaign.description;
        }
    }
}