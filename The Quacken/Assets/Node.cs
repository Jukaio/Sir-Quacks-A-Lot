using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 m_position;
    public Node m_parent;

    public Node(Vector2 p_position, Node p_parent)
    {
        m_position = p_position;
        m_parent = p_parent;
    }
}
