﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    void Start()
    {
        Service<Map_Manager>.Get().Map = this;
    }

    void Update()
    {
        
    }
}
