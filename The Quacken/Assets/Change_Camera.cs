using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Camera : MonoBehaviour
{
    private Teleport teleport;
    public Start_Menu start_menu;

    // Start is called before the first frame update
    void Start()
    {
        teleport = GetComponent<Teleport>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(Service<Game_Manager>.Get().Player.transform.position, teleport.m_end.transform.position) < 1.5f)
        {
            start_menu.camera.transform.position = start_menu.camera_points[(int)start_menu.m_index+1].transform.position;
        }
    }
}
