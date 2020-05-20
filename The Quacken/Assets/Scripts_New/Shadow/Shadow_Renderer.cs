using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shadow))]
public class Shadow_Renderer : MonoBehaviour
{
    Shadow m_shadow;

    Mesh m_mesh;
    GameObject m_mesh_object;
    public PolygonCollider2D m_mesh_collider_object;
    public Material m_shadow_material;

    private Vector3[] m_vertices;
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
        m_mesh = Utility.Extra_Mesh.Create_Mesh(out m_mesh_object, out m_mesh_collider_object,
                            "m_shadow", "m_shadow_collider",
                            "Light_Overlay", "Ignore Raycast",
                            new Material(m_shadow_material));
        m_mesh_collider_object.gameObject.tag = "lightCollider";
    }

    void LateUpdate()
    {
        var temp = m_vertices.To_Vector2_Array();
        temp[0] = temp[1];
        m_mesh_collider_object.points = temp;
        Utility.Extra_Mesh.Update_Mesh(ref m_mesh, m_vertices, m_triangles);
    }
}
