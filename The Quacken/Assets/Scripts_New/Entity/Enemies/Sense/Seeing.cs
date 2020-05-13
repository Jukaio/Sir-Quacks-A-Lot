using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // Trigger_Zone
    PolygonCollider2D m_trigger_zone;
    Vector2[] m_polygon_trigger_points;

    public Material m_test_mat;
    MeshFilter m_mesh_filter;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;
    GameObject m_mesh_object;

    int[] m_triangles;
    Vector3[] m_verts;

    void Start()
    {
        m_mesh = Utility.Extra_Mesh.Create_Mesh(out m_mesh_object, out m_mesh_renderer, "s_mesh", "Default", m_test_mat);
        m_mesh_object.transform.localPosition = Vector3.zero + Vector3.back;
        m_mesh_object.transform.parent = transform;

        m_trigger_zone = (PolygonCollider2D)gameObject.AddComponent(typeof(PolygonCollider2D));
        m_trigger_zone.isTrigger = true;
        m_polygon_trigger_points = new Vector2[(int)m_cone_width * 2 + 1];
        m_triangles = new int[((int)m_cone_width * 2) * 3];
        m_verts = new Vector3[(int)m_cone_width * 2 + 1];

    }

    void Update_Trigger_Zone(Vector2 p_view_direction)
    {
        // Make/Use a polygon collider, idiot. It's cheaper than 90 fucking raycasts!! 
        Vector2 look_direction = p_view_direction * m_cone_length;

        m_mesh_object.transform.localPosition = Vector3.zero + Vector3.back * 5.0f;
        int triangle_index = 1;
        m_polygon_trigger_points[0] = Vector2.zero;
        m_verts[0] = Vector3.zero;
        for (int i = (int)-m_cone_width; i < (int)m_cone_width; i++)
        {
            int index = i + (int)m_cone_width;
            var temp = look_direction.Rotate(i).normalized * m_cone_length;
            m_polygon_trigger_points[index + 1] = temp;
            m_verts[index + 1] = temp;
            m_triangles[index * 3 + 0] = 0;
            m_triangles[index * 3 + 1] = triangle_index;
            m_triangles[index * 3 + 2] = triangle_index + 1;
            triangle_index++;
        }
        m_triangles[m_triangles.Length - 1] = 1;
        m_mesh_renderer.sortingOrder = 2;

        m_trigger_zone.points = m_polygon_trigger_points;

        Utility.Extra_Mesh.Update_Mesh(ref m_mesh, m_verts, m_triangles);
    }

    //void Field_Of_View(Vector2 p_view_direction)
    //{
    //    // Make/Use a polygon collider, idiot. It's cheaper than 90 fucking raycasts!! 
    //    Vector2 look_direction = (Vector2)(p_view_direction * m_cone_length);

    //    m_mesh_object.transform.localPosition = Vector3.zero + Vector3.back * 5.0f;
    //    int triangle_index = 1;
    //    m_polygon_trigger_points[0] = Vector2.zero;
    //    m_verts[0] = Vector3.zero;
    //    for (int i = (int)-m_cone_width; i < (int)m_cone_width; i++)
    //    {
    //        int index = i + (int)m_cone_width;
    //        var temp = look_direction.Rotate(i).normalized * m_cone_length;
    //        m_polygon_trigger_points[index + 1] = temp;
    //        m_verts[index + 1] = temp;
    //        m_triangles[index * 3 + 0] = 0;
    //        m_triangles[index * 3 + 1] = triangle_index;
    //        m_triangles[index * 3 + 2] = triangle_index + 1;
    //        triangle_index++;
    //    }
    //    m_triangles[m_triangles.Length - 1] = 1;
    //    m_mesh_renderer.sortingOrder = 15;

    //    m_trigger_zone.points = m_polygon_trigger_points;

    //    Utility.Extra_Mesh.Update_Mesh(ref m_mesh, m_verts, m_triangles);
    //}

    bool Player_In_Vision_Range(Vector2 p_view_direction)
    {
        float view_angle = Mathf.Atan2(p_view_direction.x, p_view_direction.y);
        m_to_player_angle = Vector2.Angle(p_view_direction.normalized, m_to_player_direction.normalized);

        return m_to_player_angle > view_angle - m_cone_width && m_to_player_angle < view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    bool in_trigger_zone = false;
    public bool Sense(Vector2 p_view_direction, GameObject p_target)
    {
        m_to_player_direction = Set_Direction_To_Target(p_target);
        m_to_player_distance = Set_Distance_To_Target(p_target);

        Update_Trigger_Zone(p_view_direction);

        if (in_trigger_zone) // Only Raycast if player is in the trigger zone
        {
            var hit = Physics2D.Raycast(transform.position, m_to_player_direction * m_to_player_distance);
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
            if (hit.collider.CompareTag(p_target.tag))
            {
                in_trigger_zone = false;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Player":
                in_trigger_zone = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                in_trigger_zone = false;
                break;
        }
    }
}
