﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shadow))]
public class Shadow_Renderer : MonoBehaviour
{
    Shadow m_shadow;

    Mesh m_mesh;
    GameObject m_mesh_object;
    GameObject m_mesh_collider_object;
    public Material m_shadow_material;

    Vector3[] m_vertices;
    public Vector3[] Vertices
    {
        get { return m_vertices; }
        set { m_vertices = value; }
    }
    int[] m_triangles;
    public int[] Triangles
    {
        get { return m_triangles; }
        set { m_triangles = value; }
    }

    void Start()
    {
        m_shadow = GetComponent<Shadow>();
        m_mesh = Utility.Create_Mesh(out m_mesh_object, out m_mesh_collider_object,
                            "m_shadow", "m_shadow_collider",
                            "Light_Overlay", "Ignore Raycast",
                            new Material(m_shadow_material));
    }

    void Update()
    {
        Utility.Update_Mesh(ref m_mesh, m_vertices, m_triangles);
    }
}
