using UnityEngine;
using TMPro; 

public class WeekDisplay : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI weekText; 

    [Header("Settings")]
    public string prefix = "Week ";

    void Start()
    {
     Debug.Log("counter start fn");   
        UpdateDisplay();
    }

//when new currentWeek starts
    public void UpdateDisplay()
    {
        Debug.Log("updatedisplay fn");   

        if (weekText != null) { weekText.text = prefix + GlobalGameManager.Instance.GetCurrentWeekIndex().ToString(); }
    }
}