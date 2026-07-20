using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int resourceIndex;

    private float resourceAmount;
    private float displayedAmount;

    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    [SerializeField] private GameObject resourcePiecePrefab;
    [SerializeField] private List<ResourcePiece> resourcePieces = new List<ResourcePiece>();

    void OnEnable() { GlobalGameManager.OnUpdateTheme += OnUpdateTheme; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= OnUpdateTheme; }

    public void OnUpdateTheme() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();

        gameObject.GetComponent<SpriteRenderer>().color = menuTheme.resourceBarBackgroundColor;

        for(int i = 0; i < resourcePieces.Count; i++) { Destroy(resourcePieces[i].gameObject); }
        resourcePieces.Clear();

        MenuTheme.ResourceBarColors.Collection[] collectionArray = menuTheme.resourceBarColors;
        MenuTheme.ResourceBarColors[] resourceBarColors = collectionArray[resourceIndex].resourceBars;

        if(resourceIndex < collectionArray.Length && resourceBarColors != null && resourceBarColors.Length > 0) {
            //ResourceBarColorsCollection collection = collectionArray[resourceIndex];
            for(int i = 0; i < resourceBarColors.Length; i++) {
                GameObject newResourcePiece = Instantiate(resourcePiecePrefab);
                newResourcePiece.transform.parent = gameObject.transform;
                //newResourcePiece.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -(0.1f * (i + 1)));
                ResourcePiece newResourcePieceComponent = newResourcePiece.GetComponent<ResourcePiece>();
                    
                Week.Utilities.ResourceBarValues values = GlobalGameManager.GetCurrentWeek().resourceBars[resourceIndex].resourceBars[i];

                newResourcePieceComponent.min = values.min;
                newResourcePieceComponent.max = values.max;
                newResourcePieceComponent.fill.GetComponent<SpriteRenderer>().color = resourceBarColors[i].fill;
                newResourcePieceComponent.change.GetComponent<SpriteRenderer>().color = resourceBarColors[i].change;

                resourcePieces.Add(newResourcePieceComponent);

                newResourcePiece.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -(0.1f * (i + 1)));
                UpdateDisplay();
            }
        } //if(collection.resourceBars != null && collection.resourceBars.Length > 0) {
    }

    void Update() {
        if(resourceIndex == 0) { resourceAmount = GlobalGameManager.GetCurrentWeekIndex() + 1; }
        else { resourceAmount = LevelManager.GetResource(resourceIndex); }

        if(displayedAmount != resourceAmount) { UpdateDisplay(); }
    }

    private void UpdateDisplay() {
        displayedAmount = Mathf.Lerp(displayedAmount, resourceAmount, 3f * Time.deltaTime);
        if(Mathf.Abs(resourceAmount - displayedAmount) < 1) { displayedAmount = resourceAmount; }
        bool resourceBigger = resourceAmount > displayedAmount; //if happiness > current fill amount, set change to happiness and lerp fill to change. Otherwise, set fill to happiness and lerp change to fill.
        string displayText = prefix + resourceAmount + suffix;
        text.text = displayText;

        for(int i = 0; i < resourcePieces.Count; i++) {
            ResourcePiece resourceBar = resourcePieces[i];
            AdjustPositionAndSize(resourceBar.fill, GetProgress(resourceBigger ? displayedAmount : resourceAmount, resourceBar));
            AdjustPositionAndSize(resourceBar.change, GetProgress(resourceBigger ? resourceAmount : displayedAmount, resourceBar));
            resourceBar.text.GetComponent<TextMeshProUGUI>().text = displayText;
            resourceBar.mask.GetComponent<RectMask2D>().padding = new Vector4(0, 0, 6.2f - (GetProgress(resourceBigger ? displayedAmount : resourceAmount, resourceBar) * 6.2f), 0);
        }
    }

    private float GetProgress(float resource, ResourcePiece resourceBar) {
        return Mathf.Clamp((resource - resourceBar.min) / (resourceBar.max - resourceBar.min), 0f, 1f);
    }

    private void AdjustPositionAndSize(GameObject bar, float progress) {
        Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().size;
        bar.transform.position = new Vector3(gameObject.transform.position.x - (((spriteSize.x - 0.1f) / 2f) * (1 - progress)), gameObject.transform.position.y, bar.transform.position.z);
        bar.GetComponent<SpriteRenderer>().size = new Vector2((spriteSize.x - 0.1f) * progress, (spriteSize.y - 0.1f));
    }
}