﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunnel_enter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.transform.parent.GetComponent<UnityEngine.Tilemaps.Tilemap>().color = Color.clear;
           
        }
    }
}
