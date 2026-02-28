using System;
using TMPro;
using UnityEngine;

public class HappinessBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject background;

    private LevelManager gameManager;

    [SerializeField] ResourceType resourceType;

    private float resourceAmount;
    private float displayedAmount;

    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    [SerializeField] private ResourceBarWithOverflow[] resourceBars;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = FindFirstObjectByType<LevelManager>();


        displayedAmount = resourceAmount;

        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x - 0.1f, background.GetComponent<SpriteRenderer>().size.y - 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(resourceType) {
            case ResourceType.Happiness: resourceAmount = gameManager.GetHappiness(); break;
            case ResourceType.Money: resourceAmount = gameManager.GetMoney(); break;
        }

        displayedAmount = Mathf.Lerp(displayedAmount, resourceAmount, 3f * Time.deltaTime);
        text.text = prefix + resourceAmount + suffix;

        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x - 0.1f, background.GetComponent<SpriteRenderer>().size.y - 0.1f);

        if(resourceAmount > displayedAmount) { //if happiness > current fill amount, set change to happiness and lerp fill to change
            foreach(ResourceBarWithOverflow resourceBar in resourceBars) {
                AdjustPositionAndSize(resourceBar.fill, Mathf.Clamp((displayedAmount - resourceBar.min) / resourceBar.max, 0f, 1f), fullSize);
                AdjustPositionAndSize(resourceBar.change, Mathf.Clamp((resourceAmount - resourceBar.min) / resourceBar.max, 0f, 1f), fullSize);

            }
        } else { //otherwise, set fill to happiness and lerp change to fill
            foreach(ResourceBarWithOverflow resourceBar in resourceBars) {
                AdjustPositionAndSize(resourceBar.fill, Mathf.Clamp((resourceAmount - resourceBar.min) / resourceBar.max, 0f, 1f), fullSize);
                AdjustPositionAndSize(resourceBar.change, Mathf.Clamp((displayedAmount - resourceBar.min) / resourceBar.max, 0f, 1f), fullSize);
            }
        }

    }

    private void AdjustPositionAndSize(GameObject bar, float progress, Vector2 spriteSize) {
        bar.transform.position = new Vector3(background.transform.position.x - ((spriteSize.x / 2f) * (1- progress)), bar.transform.position.y, bar.transform.position.z);
        bar.GetComponent<SpriteRenderer>().size = new Vector2((spriteSize.x) * progress, spriteSize.y);
    }
}

[Serializable]
public class ResourceBarWithOverflow {
    public GameObject fill;
    public GameObject change;
    public float min;
    public float max;
}

public enum ResourceType {
    Happiness,
    Money
}