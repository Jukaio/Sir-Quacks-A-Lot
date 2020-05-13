using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject m_start;
    public GameObject m_end;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(Service<Game_Manager>.Get().Player.transform.position, m_start.transform.position) < 1.5f)
        {
            GameObject player = Service<Game_Manager>.Get().Player;
            player.transform.position = m_end.transform.position;
            player.GetComponent<Player_Controller>().m_spawn_position = m_end.transform.position;
        }
    }
}
