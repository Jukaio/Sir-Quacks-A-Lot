using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : Sensing
{
    [SerializeField] private float m_hearing_range = 3.0f;

    //// Internal variables
    // Player

    // to_player calculations
    float m_to_player_distance;

    void Start()
    {

    }

    public bool Hear(GameObject p_target, float p_noise_rage)
    {
        m_to_player_distance = Set_Distance_To_Target(p_target);
        return m_to_player_distance < p_noise_rage + m_hearing_range;
    }
}
