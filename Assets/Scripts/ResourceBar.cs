using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI text;
    public GameObject background;

    [SerializeField] private int resourceIndex;

    private float resourceAmount;
    private float displayedAmount;

    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    [SerializeField] private GameObject resourcePiecePrefab;
    [SerializeField] private List<ResourcePiece> resourcePieces = new List<ResourcePiece>();

    void OnEnable() { GlobalGameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= UpdateMenuObject; }

    private void Awake() {
        levelManager = FindFirstObjectByType<LevelManager>();
        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x - 0.1f, background.GetComponent<SpriteRenderer>().size.y - 0.1f);
        //UpdateMenuObject();
    }

    public void UpdateMenuObject() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();

        background.GetComponent<SpriteRenderer>().color = menuTheme.resourceBarBackgroundColor;

        for(int i = 0; i < resourcePieces.Count; i++) { Destroy(resourcePieces[i].gameObject); }
        resourcePieces.Clear();

        ResourceBarColorsCollection[] collectionArray = menuTheme.resourceBarColors;
        if(resourceIndex < collectionArray.Length) {
            ResourceBarColorsCollection collection = collectionArray[resourceIndex];
            //if(collection.resourceBars != null && collection.resourceBars.Length > 0) {
                ResourceBarColors[] resourceBarColors = collection.resourceBars;
                if(resourceBarColors != null && resourceBarColors.Length > 0) {
                    for(int i = 0; i < resourceBarColors.Length; i++) {
                        GameObject newResourcePiece = Instantiate(resourcePiecePrefab);
                        newResourcePiece.transform.parent = background.transform;
                        ResourcePiece newResourcePieceComponent = newResourcePiece.GetComponent<ResourcePiece>();
                    
                        ResourceBarValues values = GlobalGameManager.GetCurrentWeek().resourceBars[resourceIndex].resourceBars[i];

                        newResourcePieceComponent.min = values.min;
                        newResourcePieceComponent.max = values.max;

                        newResourcePieceComponent.fill.GetComponent<SpriteRenderer>().color = resourceBarColors[i].fill;
                        newResourcePieceComponent.change.GetComponent<SpriteRenderer>().color = resourceBarColors[i].change;

                        resourcePieces.Add(newResourcePieceComponent);

                        newResourcePiece.transform.position = Vector3.zero;
                    }
                }
            //}
        }
    }

    void Update() {
        if(resourceIndex == 0) { resourceAmount = GlobalGameManager.GetCurrentWeekIndex() + 1; }
        else { resourceAmount = levelManager.GetResource(resourceIndex); }
        
        displayedAmount = Mathf.Lerp(displayedAmount, resourceAmount, 3f * Time.deltaTime);
        if(Mathf.Abs(resourceAmount - displayedAmount) < 1) { displayedAmount = resourceAmount; }
        text.text = prefix + resourceAmount + suffix;

        Vector2 fullSize = new Vector2(background.GetComponent<SpriteRenderer>().size.x - 0.1f, background.GetComponent<SpriteRenderer>().size.y - 0.1f);

        //if happiness > current fill amount, set change to happiness and lerp fill to change
        //otherwise, set fill to happiness and lerp change to fill
        bool resourceBigger = resourceAmount > displayedAmount;
        for(int i = 0; i < resourcePieces.Count; i++) {
            ResourcePiece resourceBar = resourcePieces[i];

            resourceBar.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, -1f -(0.3f * (i + 1)));

            AdjustPositionAndSize(resourceBar.fill, GetProgress(resourceBigger ? displayedAmount : resourceAmount, resourceBar), fullSize);
            AdjustPositionAndSize(resourceBar.change, GetProgress(resourceBigger ? resourceAmount : displayedAmount, resourceBar), fullSize);
            resourceBar.text.GetComponent<TextMeshProUGUI>().text = prefix + resourceAmount + suffix;
            resourceBar.mask.GetComponent<RectMask2D>().padding = new Vector4(0, 0, 6.2f - (GetProgress(resourceBigger ? displayedAmount : resourceAmount, resourceBar) * 6.2f), 0);
        }
    }

    private float GetProgress(float resource, ResourcePiece resourceBar) {
        return Mathf.Clamp((resource - resourceBar.min) / (resourceBar.max - resourceBar.min), 0f, 1f);
    }

    private void AdjustPositionAndSize(GameObject bar, float progress, Vector2 spriteSize) {
        bar.transform.position = new Vector3(background.transform.position.x - ((spriteSize.x / 2f) * (1 - progress)), gameObject.transform.position.y, bar.transform.position.z);
        bar.GetComponent<SpriteRenderer>().size = new Vector2((spriteSize.x) * progress, spriteSize.y);
    }
}