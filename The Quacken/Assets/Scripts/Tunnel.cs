using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{
    bool inTunnel;
    public GameObject renderer;

    private void Start()
    {
        inTunnel = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tunnel" && !inTunnel)
        {
            renderer.GetComponent<UnityEngine.Tilemaps.Tilemap>().color = Color.clear;
            inTunnel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "tunnel" && inTunnel)
        {
            renderer.GetComponent<UnityEngine.Tilemaps.Tilemap>().color = Color.white;
            inTunnel = false;
        }
    }
}
