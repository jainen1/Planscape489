using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {
    [SerializeField] private BackgroundType type;

    [SerializeField] private GameObject backgroundLayerPrefab;
    [SerializeField] private List<GameObject> backgroundLayers = new List<GameObject>();

    void OnEnable() { GlobalGameManager.OnUpdateTheme += UpdateMenuObject; }
    void OnDisable() { GlobalGameManager.OnUpdateTheme -= UpdateMenuObject; }

    public void Awake () {
        if(gameObject.GetComponent<SpriteRenderer>() != null) {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void UpdateMenuObject() {
        MenuTheme menuTheme = GlobalGameManager.GetCurrentMenuTheme();

        for(int i = 0; i < backgroundLayers.Count; i++) { Destroy(backgroundLayers[i].gameObject); }
        backgroundLayers.Clear();

        BackgroundLayer[] themeLayers;
        if(type == BackgroundType.Menu) { themeLayers = menuTheme.menuBackgroundLayers; }
        else { themeLayers = menuTheme.levelBackgroundLayers; }

        for(int i = 0; i < themeLayers.Length; i++) {
            GameObject newBackgroundLayer = Instantiate(backgroundLayerPrefab);
            newBackgroundLayer.transform.parent = gameObject.transform;

            newBackgroundLayer.GetComponent<SpriteRenderer>().sprite = themeLayers[i].sprite;
            newBackgroundLayer.GetComponent<SpriteRenderer>().color = themeLayers[i].color;
            newBackgroundLayer.transform.position = new Vector3(themeLayers[i].position.x, themeLayers[i].position.y, gameObject.transform.position.z + (0.005f * i));
            newBackgroundLayer.transform.rotation = themeLayers[i].rotation;
            newBackgroundLayer.transform.localScale = themeLayers[i].scale;

            backgroundLayers.Add(newBackgroundLayer);
        }
    }
}

public enum BackgroundType {
    Menu,
    Level
}