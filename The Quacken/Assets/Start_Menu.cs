using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Menu : MonoBehaviour
{
    public GameObject camera;
    public GameObject[] camera_points;

    public GameObject[] m_points;

    public GameObject m_closed_door;

    public Teleport m_teleport;
    public enum Rooms
    {
        DEFAULT = -1,
        GAME, // 0
        EXIT // 1

    }
    public Rooms m_index = Rooms.DEFAULT;
    private Rooms previous_index = Rooms.DEFAULT;

    public SpriteRenderer room_type;

    public Sprite[] room_type_sprites;

    public SpriteRenderer m_right_button;
    public Sprite m_right_pressed;
    public Sprite m_right_released;
    bool m_pressed_right;
    bool m_prev_press_right;

    public SpriteRenderer m_left_button;
    public Sprite m_left_pressed;
    public Sprite m_left_released;
    bool m_pressed_left;
    bool m_prev_press_left;

    GameObject m_player;

    AudioSource m_source;
    public AudioClip m_button_press;

    private void Start()
    {
        m_source = gameObject.AddComponent<AudioSource>();
        m_source.loop = false;
        m_source.playOnAwake = false;

        m_player = Service<Game_Manager>.Get().Player.gameObject;

        camera.transform.position = camera_points[0].transform.position;
        //Scene_Manager.Load_Level(1); // <- Loads a certain scene from the build settings
                                       // 0 = Game_Managment; 1 = Start_Menu; 2 = Level_One

        /*
        Interactive Start Screen:
        1. Movement - Duck moves already, so this part is done
        2. If the duck is within a certain rectangle, the index value changes 
        3. If the duck is not within any rectangle, the index is -1
        4. Left button decreases by one 
        5. Right button advances by one
        */
    }

    private void Update()
    {
        if (m_index != previous_index)
        {
            room_type.sprite = room_type_sprites[(int)m_index];
            m_teleport.m_end = m_points[(int)m_index];
        }

        Vector2 player_pos = m_player.transform.position;
        Vector2 left_button_pos = m_left_button.transform.position;
        Vector2 right_button_pos = m_right_button.transform.position;

        float size = 1;

        m_pressed_left = (player_pos.x < left_button_pos.x + size && player_pos.x > left_button_pos.x - size &&
                          player_pos.y < left_button_pos.y + size && player_pos.y > left_button_pos.y - size);

        m_pressed_right = (player_pos.x < right_button_pos.x + size && player_pos.x > right_button_pos.x - size &&
                          player_pos.y < right_button_pos.y + size && player_pos.y > right_button_pos.y - size);

        if (m_pressed_left && !m_prev_press_left)
        {
            if (m_index == Rooms.DEFAULT)
                Unlock_Door();

            if (m_source.clip != m_button_press)
                m_source.clip = m_button_press;

            m_source.Play();

            m_left_button.sprite = m_left_pressed;
            m_index--;
            if ((int) m_index < 0)
                m_index = (Rooms) 1;
        }
        else if (m_pressed_left == false)
        {
            m_left_button.sprite = m_left_released;
        }

        if (m_pressed_right && !m_prev_press_right)
        {
            if (m_index == Rooms.DEFAULT)
                Unlock_Door();

            if (m_source.clip != m_button_press)
                m_source.clip = m_button_press;

            m_source.Play();

            m_right_button.sprite = m_right_pressed;
            m_index++;
            if ((int)m_index > 1)
                m_index = (Rooms) 0;
        }
        else if (m_pressed_right == false)
        {
            m_right_button.sprite = m_right_released;
        }

        foreach(var point in camera_points)
        {
            if(m_player.transform.position.Distancef(point.transform.position) < 4.0f)
            {
                camera.transform.position = point.transform.position;
            }
        }



        m_prev_press_left = m_pressed_left;
        m_prev_press_right = m_pressed_right;
        
    }

    void Unlock_Door()
    {
        m_closed_door.SetActive(false);
    }

}
