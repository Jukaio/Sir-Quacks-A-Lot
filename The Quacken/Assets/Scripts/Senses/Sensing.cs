﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensing : MonoBehaviour
{
    public float Set_Distance_To_Target(GameObject p_target)
    {
        return Vector2.Distance(transform.position, p_target.transform.position);
    }

    public Vector2 Set_Direction_To_Target(GameObject p_target)
    {
        return (p_target.transform.position - transform.position).normalized;
    }
}
