using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    SpriteRenderer renderer;
    public int topLayer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sortingOrder = topLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "mapAbove")
        {
            //Color color = Color.white;
            //color.a = 0.5f;
            //collision.gameObject.transform.parent.GetComponent<UnityEngine.Tilemaps.Tilemap>().color = color;
            renderer.sortingOrder = 9;
        }
        if (collision.tag == "mapBellow")
        {
            //Color color = Color.white;
            //collision.gameObject.transform.parent.GetComponent<UnityEngine.Tilemaps.Tilemap>().color = color;
            renderer.sortingOrder = topLayer;
        }
    }
}


