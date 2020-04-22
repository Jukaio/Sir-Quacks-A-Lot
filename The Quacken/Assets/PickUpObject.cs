using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform theDest;

    void OnTriggerEnter2D(Collider2D collision)
    {
        this.transform.position = theDest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.transform.parent = null;
    }
}
