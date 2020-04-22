using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Path : MonoBehaviour
{
    public GameObject m_trail_start;
    public Vector2 m_target;

    Rigidbody2D m_rb;

    private void OnEnable()
    {
        m_target = m_trail_start.transform.position;
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        m_rb.velocity = (m_target - (Vector2)transform.position).normalized;
    }

    bool found_trail = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Smell" && !found_trail)
        {
            m_trail_start = collision.gameObject;
            found_trail = true;
        }
    }
}
