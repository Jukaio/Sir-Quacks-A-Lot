using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabThrowScript : MonoBehaviour
{
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 2f;
    public Transform holdpoint;
    public float throwForce = 0f;
    public LayerMask notPickUpObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!grabbed)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
                
                if (hit.collider != null && hit.collider.tag=="pickupObject")
                {
                    grabbed = true;
                }
               
                //can grab
            }
            else if(!Physics2D.OverlapPoint(holdpoint.position, notPickUpObject))
            {
                grabbed = false;

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwForce;
                }
                //throw
            }
        }
        if (grabbed)
        {
            hit.collider.gameObject.transform.position = holdpoint.position;
            //grabbing
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position+Vector3.right * transform.localScale.x*distance);
    }
}
