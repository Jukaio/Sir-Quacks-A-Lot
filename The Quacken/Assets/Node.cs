using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour
{
    public Vector2 m_position;
    public Node[] neighbours_;

    void Start()
    {
        Grid_Manager.Subscribe(this);
    }
}
