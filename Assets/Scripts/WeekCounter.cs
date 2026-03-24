using UnityEngine;
using TMPro; 

public class WeekDisplay : MonoBehaviour
{
    [Header("References")]
    public Week currentWeek; 
    public TextMeshProUGUI weekText; 

    [Header("Settings")]
    public string prefix = "Week ";

    void Start()
    {
     Debug.Log("counter start fn");   
        UpdateDisplay();
    }

//when new week starts
    public void UpdateDisplay()
    {
        Debug.Log("updatedisplay fn");   

        if (currentWeek != null && weekText != null)
        {
            Debug.Log("updatedisplay if passed");  
            Debug.Log (currentWeek); 
            weekText.text = prefix + currentWeek.weekNumber.ToString();
        }
    }
}