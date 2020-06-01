﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    #region Singleton
    public static GoldCounter instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int gold = 0;
    public delegate void OnGoldChange(int currentGold);
    public OnGoldChange onGoldChange; 

    public void Earn(int gold)
    {
        this.gold += gold;
        onGoldChange(this.gold);
    }

    public void Spend(int gold)
    {
        if (this.gold >= gold)
        {
            this.gold -= gold;
            onGoldChange(this.gold);
        }
        else
        {
            Debug.Log("Not enough gold");
        }
    }
}
