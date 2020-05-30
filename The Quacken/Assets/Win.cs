using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    private bool m_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_flag && Vector2.Distance(Service<Game_Manager>.Get().Player.transform.position, transform.position) < 1.5f)
        {
            m_flag = true;
            StartCoroutine(Scene_Manager.Load_Level(3));
            StartCoroutine(Scene_Manager.Unload_Level(2));
        }
    }
}
