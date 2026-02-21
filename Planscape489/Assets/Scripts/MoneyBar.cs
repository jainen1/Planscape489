using UnityEngine;
using TMPro;

public class MoneyBar : MonoBehaviour
{
    private TextMeshProUGUI text;
    private LevelManager gameManager;
    string origin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        gameManager = FindFirstObjectByType<LevelManager>();
        origin = text.text;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = origin + " " + gameManager.GetMoney();
    }
}