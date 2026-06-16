using UnityEngine;

public interface ReceivesThemeUpdates
{
    void OnEnable () { GlobalGameManager.OnUpdateTheme += OnThemeUpdate; }
    void OnDisable () { GlobalGameManager.OnUpdateTheme -= OnThemeUpdate; }

    void OnThemeUpdate();
    Color GetMainColor();
}