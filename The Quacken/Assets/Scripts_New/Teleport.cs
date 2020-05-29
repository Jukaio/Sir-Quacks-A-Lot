using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject m_start;
    public GameObject m_end;

    AudioSource m_source;
    public AudioClip m_transition_clip;
    

    // Start is called before the first frame update
    void Start()
    {
        m_source = gameObject.AddComponent<AudioSource>();
        m_source.loop = false;
        m_source.playOnAwake = false;
        m_source.clip = m_transition_clip;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(Service<Game_Manager>.Get().Player.transform.position, m_start.transform.position) < 1.5f)
        {
            if (!running)
                StartCoroutine(Transition());
        }
    }

    bool running = false;
    IEnumerator Transition()
    {
        running = true;
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(0.25f);


        Time.timeScale = 1.0f;

        GameObject player = Service<Game_Manager>.Get().Player;
        player.transform.position = m_end.transform.position;
        player.GetComponent<Player_Controller>().m_spawn_position = m_end.transform.position;
        m_source.Play();
        running = false;
    }
}
