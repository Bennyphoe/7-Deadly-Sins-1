﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopManager : MonoBehaviour
{

    [SerializeField]
    HotKeyBar hotKeyBar;
    // Start is called before the first frame update
    void Start()
    {
        hotKeyBar.DisableAllSkills();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
