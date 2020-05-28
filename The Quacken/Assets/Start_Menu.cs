using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Menu : MonoBehaviour
{
    public GameObject camera;
    public GameObject[] camera_points;
    public GameObject[] m_points;

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

    private void Start()
    {
        
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
            m_right_button.sprite = m_right_pressed;
            m_index++;
            if ((int)m_index > 1)
                m_index = (Rooms) 0;
        }
        else if (m_pressed_right == false)
        {
            m_right_button.sprite = m_right_released;
        }

        m_prev_press_left = m_pressed_left;
        m_prev_press_right = m_pressed_right;
        
    }

}
