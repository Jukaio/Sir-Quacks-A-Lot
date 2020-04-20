﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Manager : MonoBehaviour
{
    static private Grid_Manager m_instance;
    public Node[] m_nodes;

    private void Awake()
    {
        m_instance = this;
        m_instance.m_nodes = new Node[0];
    }

    private void Update()
    {

    }

    public Node[,] m_grid;
    public Node most_top;
    public Node most_right;
    public Node most_bottom;
    public Node most_left;
    public static void Subscribe(Node p_node)
    {
        p_node.m_position.x = p_node.transform.position.x;
        p_node.m_position.y = p_node.transform.position.y;

        if (m_instance.m_nodes.Length == 0)
        {
            m_instance.most_top = p_node;
            m_instance.most_right = p_node;
            m_instance.most_bottom = p_node;
            m_instance.most_left = p_node;
        }
        else if (p_node.transform.position.x < m_instance.most_left.transform.position.x)
            m_instance.most_left = p_node;
        else if (p_node.transform.position.x > m_instance.most_right.transform.position.x)
            m_instance.most_right = p_node;
        else if (p_node.transform.position.y > m_instance.most_top.transform.position.y)
            m_instance.most_top = p_node;
        else if (p_node.transform.position.y < m_instance.most_bottom.transform.position.y)
            m_instance.most_bottom = p_node;

        Node[] temp = m_instance.m_nodes;
        m_instance.m_nodes = new Node[temp.Length + 1];
        for (int i = 0; i < temp.Length; i++)
            m_instance.m_nodes[i] = temp[i];
        m_instance.m_nodes[m_instance.m_nodes.Length - 1] = p_node;

        if (m_instance.m_nodes.Length == m_instance.transform.childCount)
        {
            int offset_x = -(int)(m_instance.most_left.m_position.x);
            int offset_y = -(int)(m_instance.most_bottom.m_position.y);
            int grid_width = (int)((Mathf.Abs(m_instance.most_left.m_position.x) + Mathf.Abs(m_instance.most_right.m_position.x)));
            int grid_height = (int)((Mathf.Abs(m_instance.most_top.m_position.y) + Mathf.Abs(m_instance.most_bottom.m_position.y)));

            m_instance.m_grid = new Node[grid_width + 1, grid_height + 1];

            Debug.Log(offset_x + " " + offset_y + " " + (grid_width + 1) + " " + (grid_height + 1));

            for(int i = 0; i < m_instance.m_nodes.Length; i++)
            {
                int x = (int)m_instance.m_nodes[i].m_position.x + offset_x;
                int y = (int)m_instance.m_nodes[i].m_position.x + offset_y;

                Debug.Log(x + " " + y + " " + i, m_instance.m_nodes[i].gameObject);

                //m_instance.m_grid[x, y] = m_instance.m_nodes[i];
            }
        }
    }
}