using UnityEngine;
using System;

public class TimeHand : MonoBehaviour
{
    [Header("Manual Calibration")]
    public float topY = -156.1f;
    public float totalHeight = 899.1f;
    public float firstColumnX = 383f;
    public float columnWidth = 179.2f;
    [Header("Hours")]
    public float startHour = 6f;
    public float endHour = 22f;

    // [Header("Vertical Movement (Time)")]
    // public float topY = -156.1f;          // Pos Y at 6 AM
    // public float totalHeight = 899.1f; 
    // public int startHour = 6;
    // public int endHour = 22;

    // [Header("Horizontal Movement (Days)")]
    // public float firstColumnX = 381.8f;  // Pos X for Sunday
    // public float columnWidth = 179.2f; // Distance between columns

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        DateTime now = DateTime.Now;

        // 1. Calculate EXACT minutes passed today
        // This gives us a number like 10.33 for 10:20 AM
        float currentHourDecimal = now.Hour + (now.Minute / 60f) + (now.Second / 3600f);
        
        // 2. Calculate the progress between 6 and 22 (10 PM)
        float ratio = (currentHourDecimal - startHour) / (endHour - startHour);
        
        // 3. Keep it within the bounds
        ratio = Mathf.Clamp01(ratio);

        // 4. Calculate Vertical (Y)
        float newY = topY - (ratio * totalHeight);

        // 5. Calculate Horizontal (X) - DayOfWeek: Sunday=0, Monday=1, etc.
        int dayIndex = (int)now.DayOfWeek; 
        float newX = firstColumnX + (dayIndex * columnWidth);

        // 6. Apply to the UI
        rect.anchoredPosition = new Vector2(newX, newY);

        //debug:
        if (Time.frameCount % 60 == 0) // Only logs once per second
        {
            Debug.Log($"Time: {currentHourDecimal} | Ratio: {ratio} | NewY: {newY}");
        }
    }

    // void Update()
    // {
    //     DateTime now = DateTime.Now;

    //     // --- 1. Calculate Vertical (Y) Position ---
    //     float currentMin = (now.Hour * 60) + now.Minute + (now.Second / 60f);
    //     float startMin = startHour * 60;
    //     float endMin = endHour * 60;
        
    //     float vRatio = Mathf.Clamp01((currentMin - startMin) / (endMin - startMin));
    //     float newY = topY - (vRatio * totalHeight);

    //     // --- 2. Calculate Horizontal (X) Position ---
    //     // DayOfWeek returns 0 for Sunday, 1 for Monday, etc.
    //     int dayIndex = (int)now.DayOfWeek; 
    //     float newX = firstColumnX + (dayIndex * columnWidth);

    //     // --- 3. Apply ---
    //     rect.anchoredPosition = new Vector2(newX, newY);
    // }
}