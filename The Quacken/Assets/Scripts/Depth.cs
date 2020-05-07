﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sortingOrder = 12;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "mapAbove")
        {
            renderer.sortingOrder = 9;
        }
        if (collision.tag == "mapBellow")
        {
            renderer.sortingOrder = 12;
        }
        //if (collision.tag == "wallSort")
        //{
        //    renderer.sortingOrder = 2;
        //}
        if (collision.tag == "tunnel")
        {
            renderer.color = Color.clear;
        }
    }
}
