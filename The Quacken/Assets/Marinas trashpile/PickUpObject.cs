using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform theDest;

    public void DoInteraction()
    {
        this.transform.position = theDest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
    }

    public void ObjectDrop()
    {
        this.transform.parent = null;
    }
}
