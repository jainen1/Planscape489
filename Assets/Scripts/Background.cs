using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {
    [SerializeField] private BackgroundType type;
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

        MenuTheme.BackgroundLayer[] themeLayers;
        if(type == BackgroundType.Menu) { themeLayers = menuTheme.menuBackgroundLayers; }
        else { themeLayers = menuTheme.levelBackgroundLayers; }

        for(int i = 0; i < themeLayers.Length; i++) {
            GameObject newBackgroundLayer = new GameObject("BackgroundLayer"+i);
            newBackgroundLayer.transform.parent = gameObject.transform;
            newBackgroundLayer.transform.position = new Vector3(themeLayers[i].position.x, themeLayers[i].position.y, gameObject.transform.position.z + (0.005f * i));
            newBackgroundLayer.transform.rotation = themeLayers[i].rotation;
            newBackgroundLayer.transform.localScale = themeLayers[i].scale;

            SpriteRenderer spriteRenderer = (SpriteRenderer) newBackgroundLayer.AddComponent(typeof(SpriteRenderer));
            spriteRenderer.sprite = themeLayers[i].sprite;
            spriteRenderer.color = themeLayers[i].color;

            backgroundLayers.Add(newBackgroundLayer);
        }
    }
}

public enum BackgroundType {
    Menu,
    Level
}