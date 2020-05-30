using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animate_End_Screen : MonoBehaviour
{
    public Sprite[] m_sprites;
    private Image m_image;
    int m_index;
    public float m_duration = 0.2f;
    float m_timer;
    private bool m_flag = false;

    public Text m_time_text;
    public Text m_death_text;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();

        m_time_text.text = Mathf.RoundToInt(Player_Controller.m_time_passed).ToString();
        m_death_text.text = Player_Controller.m_times_caught.ToString();


        m_index = 0;
        m_image.sprite = m_sprites[m_index];
        m_timer = m_duration;

        

    }

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;

        if(m_timer < 0.0f)
        {
            m_index++;
            if (!(m_index < m_sprites.Length))
                m_index = 0;

            m_image.sprite = m_sprites[m_index];
            m_timer = m_duration;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !m_flag)
        {
            m_flag = true;
            StartCoroutine(Scene_Manager.Load_Level(1));
            StartCoroutine(Scene_Manager.Unload_Level(3));
        }
    }
}
