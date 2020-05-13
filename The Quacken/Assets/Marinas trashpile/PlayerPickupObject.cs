using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupObject : MonoBehaviour
{
    public GameObject currInterObj = null;
    private bool isHolding = false;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currInterObj.SendMessage("DoInteraction");
            isHolding = true;
           
        }
        
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pickupObject"))
        {
            Debug.Log("You can now pick up the object!");
            currInterObj = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("pickupObject"))
        {
            if(other.gameObject == currInterObj)
            {
                currInterObj = null;
            }
        }
    }
}
