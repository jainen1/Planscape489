using System;
using UnityEngine;

[Serializable]
public class GameSave {
    public string currentCampaign;
    public int week;

    public GameSave() {
        this.currentCampaign = null;
        this.week = 0;
    }
}