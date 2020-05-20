using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Sensing : MonoBehaviour
{
    public GameObject m_entity;

    public float Set_Distance_To_Target(Vector2 p_from, GameObject p_target)
    {
        return Vector2.Distance(p_from, p_target.transform.position);
    }

    public Vector2 Set_Direction_To_Target(Vector2 p_from, GameObject p_target)
    {
        return ((Vector2)p_target.transform.position - p_from).normalized;
    }

    public void Set_Body(GameObject p_entity)
    {
        m_entity = p_entity;
    }

    // By default the sense is false - If not inheritated the sense counts as "broken"
}
