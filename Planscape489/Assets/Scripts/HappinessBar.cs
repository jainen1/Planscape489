using UnityEngine;
using TMPro;

public class HappinessBar : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject fill;
    [SerializeField] private GameObject change;

    [SerializeField] private GameObject overflow;
    [SerializeField] private GameObject overflowChange;

    [SerializeField] private TextMeshProUGUI text;
    private LevelManager gameManager;
    string origin;

    private float displayedAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = FindFirstObjectByType<LevelManager>();
        origin = text.text;
        displayedAmount = gameManager.GetHappiness();

        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x - 0.1f, background.GetComponent<SpriteRenderer>().size.y - 0.1f);
        AdjustPositionAndSize(fill, Mathf.Clamp(gameManager.GetHappiness() / 100f, 0f, 1f), fullSize);
        AdjustPositionAndSize(change, Mathf.Clamp(gameManager.GetHappiness() / 100f, 0f, 1f), fullSize);

        AdjustPositionAndSize(overflow, Mathf.Clamp((gameManager.GetHappiness() - 100) / 100f, 0, 1), fullSize);
        AdjustPositionAndSize(overflowChange, Mathf.Clamp((gameManager.GetHappiness() - 100) / 100f, 0, 1), fullSize);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x-0.1f, background.GetComponent<SpriteRenderer>().size.y-0.1f);
        text.text = origin + " " + gameManager.GetHappiness() + "%";

        if(displayedAmount > gameManager.GetHappiness()) {
            AdjustPositionAndSize(fill, Mathf.Clamp(gameManager.GetHappiness() / 100f, 0f, 1f), fullSize);
            AdjustPositionAndSize(overflow, Mathf.Clamp((gameManager.GetHappiness() - 100) / 100f, 0, 1), fullSize);

            AdjustPositionAndSize(change, Mathf.Clamp(displayedAmount / 100f, 0f, 1f), fullSize);
            AdjustPositionAndSize(overflowChange, Mathf.Clamp((displayedAmount - 100) / 100f, 0f, 1f), fullSize);
        }
        else {
            AdjustPositionAndSize(change, Mathf.Clamp(gameManager.GetHappiness() / 100f, 0f, 1f), fullSize);
            AdjustPositionAndSize(overflowChange, Mathf.Clamp((gameManager.GetHappiness() - 100) / 100f, 0, 1), fullSize);

            AdjustPositionAndSize(fill, Mathf.Clamp(displayedAmount / 100f, 0f, 1f), fullSize);
            AdjustPositionAndSize(overflow, Mathf.Clamp((displayedAmount - 100) / 100f, 0f, 1f), fullSize);
        }

        displayedAmount = Mathf.Lerp(displayedAmount, gameManager.GetHappiness(), 3f * Time.deltaTime);

        //if happiness > current fill amount, set change to happiness and lerp fill to change
        //otherwise, set fill to happiness and lerp change to fill
    }

    private void AdjustPositionAndSize(GameObject bar, float progress, Vector2 spriteSize) {
        bar.transform.position = new Vector3(background.transform.position.x - ((spriteSize.x / 2f) * (1- progress)), bar.transform.position.y, bar.transform.position.z);
        bar.GetComponent<SpriteRenderer>().size = new Vector2((spriteSize.x) * progress, spriteSize.y);
    }
}