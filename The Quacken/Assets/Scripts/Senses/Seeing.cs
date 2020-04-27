using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeing : Sensing
{
    [SerializeField] private float m_cone_width;
    [SerializeField] private float m_cone_length;

    //// Internal variables
    // Player

    // to_player calculations
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;
    RaycastHit2D m_to_player_ray_hit = new RaycastHit2D();
    // !to_player calculations

    public Material m_test_mat;
    MeshFilter m_mesh_filter;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;
    GameObject m_mesh_object;

    void Start()
    {
        //Create_Mesh();
    }

    void Create_Mesh()
    {
        m_mesh_object = new GameObject("m_cone");
       //m_mesh_object.layer = LayerMask.NameToLayer("Light_Overlay");

        m_mesh_object.transform.position = Vector3.back;

        m_mesh = new Mesh();
        m_mesh_renderer = m_mesh_object.AddComponent<MeshRenderer>();
        m_mesh_filter = m_mesh_object.AddComponent<MeshFilter>();

        m_mesh_filter.sharedMesh = m_mesh;

        Material material = new Material(m_test_mat);
        m_mesh_renderer.material = material;
    }

    bool Player_In_Vision_Range(Vector2 p_view_direction)
    {
        float view_angle = Mathf.Atan2(p_view_direction.x, p_view_direction.y);

        m_to_player_angle = Vector2.Angle(p_view_direction.normalized, m_to_player_direction.normalized);

        Vector2 look_direction = (Vector2)(p_view_direction * m_cone_length);
        Debug.DrawLine(transform.position, (Vector2)transform.position + look_direction.Rotate(-m_cone_width), Color.red);
        Debug.DrawLine(transform.position, (Vector2)transform.position + look_direction.Rotate(m_cone_width), Color.red);
        Debug.DrawLine((Vector2)transform.position + look_direction.Rotate(-m_cone_width), (Vector2)transform.position + look_direction, Color.red);
        Debug.DrawLine((Vector2)transform.position + look_direction, (Vector2)transform.position + look_direction.Rotate(m_cone_width), Color.red);

        //Vector3[] temp_vert = { transform.position,
        //                        look_direction.Rotate(-m_cone_width) + (Vector2)transform.position,
        //                        look_direction + (Vector2)transform.position,
        //                        look_direction.Rotate(m_cone_width) + (Vector2)transform.position};
        //int[] temp_tris = { 0, 1, 2, 0, 2, 3 };

        //m_mesh.Clear();
        //m_mesh.vertices = temp_vert;
        //m_mesh.triangles = temp_tris;



        return m_to_player_angle > view_angle - m_cone_width && m_to_player_angle < view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    public bool Sense(Vector2 p_view_direction, GameObject p_target)
    {
        m_to_player_direction = Set_Direction_To_Target(p_target);
        m_to_player_distance = Set_Distance_To_Target(p_target);

        if (Player_In_Vision_Range(p_view_direction))
        {
            m_to_player_ray_hit = Physics2D.Raycast(transform.position, m_to_player_direction * m_to_player_distance);
            if (m_to_player_ray_hit.collider.CompareTag(p_target.tag))
                return true;
        }
        return false;
    }
}
