using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampaignListItem : MonoBehaviour
{
    public Campaign campaign;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Image shape;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    private void Start() {
        if(title != null) {
            thumbnail.sprite = campaign.thumbnail;
            shape.color = campaign.accentColor;

            title.text = campaign.title;
            description.text = campaign.description + "\n\nWeeks: <b>" + campaign.weeks.Length + "</b>\nDifficulty: <b>" + campaign.difficulty + "</b>";
        }
    }

    public void OnClickSelect() {
        GlobalGameManager.SetCampaignAndPlay(campaign);
    }
}