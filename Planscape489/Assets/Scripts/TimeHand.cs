using UnityEngine;
using System;

public class TimeHandUI : MonoBehaviour
{
    [Header("Calendar Settings")]
    public float totalDistance = 4.45f; // The distance it travels from 6AM to 10PM
    public int startHour = 6;
    public int endHour = 22;

    private RectTransform rect;
    private Vector3 initialLocalPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        // Record the position you set in the Editor as 6:00 AM
        initialLocalPos = rect.localPosition;
    }

    void Update()
    {
        TimeSpan now = DateTime.Now.TimeOfDay;
        
        // 1. Calculate progress (0.0 at 6AM, 1.0 at 10PM)
        float currentMinutes = (float)now.TotalMinutes;
        float startMinutes = startHour * 60;
        float endMinutes = endHour * 60;
        
        float ratio = (currentMinutes - startMinutes) / (endMinutes - startMinutes);
        
        // 2. Clamp it so it stays on the calendar
        ratio = Mathf.Clamp01(ratio);

        // 3. Move it DOWN from the starting position
        // We modify localPosition so we don't break the anchor/stretch settings
        float newY = initialLocalPos.y - (ratio * totalDistance);
        rect.localPosition = new Vector3(initialLocalPos.x, newY, initialLocalPos.z);
    }
}