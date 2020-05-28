using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class To_Game : MonoBehaviour
{
    private bool m_flag = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && m_flag == false)
        {
            m_flag = true;
            Scene_Manager.Unload_Level(1);
            Scene_Manager.Load_Level(2);
        }   
    }
}
