using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Seeing : Sensing
{
    [SerializeField] public float m_cone_width;
    [SerializeField] public float m_cone_length;
    [SerializeField] public float m_sense_speed;

    //// Internal variables
    // Player

    // to_player calculations
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;
    // !to_player calculations

    // Trigger_Zone
    public PolygonCollider2D m_trigger_zone;
    Vector2[] m_polygon_trigger_points;

    public Material m_test_mat;
    public Material m_feedback_test_mat;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;

    Mesh m_feedback_mesh;
    MeshFilter m_feedback_mesh_filter;
    MeshRenderer m_feedback_mesh_renderer;
    float m_feedback_Factor;

    Color fill_feedback;
    Color base_feedback;

    public bool full_Feedback
    {
        get => m_feedback_Factor >= 1;
    }

    public GameObject m_mesh_object;
    public GameObject m_feedback_mesh_object;

    int[] m_triangles;
    Vector3[] m_verts;
    Vector3[] m_feedback_verts;

    void Start()
    {
        m_mesh = Utility.Extra_Mesh.Create_Mesh(out m_mesh_object, out m_mesh_renderer, "s_seeing_mesh", "Ignore Raycast", m_test_mat);
        m_feedback_mesh = Utility.Extra_Mesh.Create_Mesh(out m_feedback_mesh_object, out m_feedback_mesh_renderer, "s_seeing_feedback_mesh", "Ignore Raycast", m_feedback_test_mat);
        
        m_mesh_object.transform.localPosition = Vector3.zero + Vector3.back;
        m_mesh_object.transform.parent = transform;

        m_feedback_mesh_object.transform.localPosition = Vector3.zero + Vector3.back;
        m_feedback_mesh_object.transform.parent = transform;


        m_trigger_zone = (PolygonCollider2D)gameObject.AddComponent(typeof(PolygonCollider2D));
        m_trigger_zone.isTrigger = true;
        m_polygon_trigger_points = new Vector2[(int)m_cone_width * 2 + 1];
        m_triangles = new int[((int)m_cone_width * 2) * 3];
        m_feedback_verts = new Vector3[(int)m_cone_width * 2 + 1];
        m_verts = new Vector3[(int)m_cone_width * 2 + 1];

        base_feedback = m_mesh_renderer.material.color;
        fill_feedback = m_feedback_mesh_renderer.material.color;

        m_feedback_mesh_renderer.material.color = (fill_feedback * 0.0f);
        m_mesh_renderer.material.color = (base_feedback * 0.0f);
    }

    void Update_Trigger_Zone(Vector2 p_view_direction)
    {
        // Make/Use a polygon collider, idiot. It's cheaper than 90 fucking raycasts!! 
        Vector2 look_direction = p_view_direction * m_cone_length;
        Vector2 position = m_entity.transform.position;
        Vector2 parent_position = transform.parent.position;
        Vector2 local_position = transform.localPosition;

        int triangle_index = 1;

        m_polygon_trigger_points[0] = position - parent_position;
        m_feedback_verts[0] = position + local_position;
        m_verts[0] = position + local_position;

        for (int i = (int)-m_cone_width; i < (int)m_cone_width; i++)
        {
            int index = i + (int)m_cone_width;
            var temp = look_direction.Rotate(i).normalized * m_cone_length;

            m_polygon_trigger_points[index + 1] = temp + position - parent_position;
            m_verts[index + 1] = temp + position + local_position;
            m_feedback_verts[index + 1] = (temp * m_feedback_Factor) + position + local_position;

            m_triangles[index * 3 + 0] = 0;
            m_triangles[index * 3 + 1] = triangle_index;
            m_triangles[index * 3 + 2] = triangle_index + 1;
            triangle_index++;
        }
        m_triangles[m_triangles.Length - 1] = triangle_index - 1;
        m_mesh_renderer.sortingOrder = 2;
        m_feedback_mesh_renderer.sortingOrder = 3;

        m_trigger_zone.points = m_polygon_trigger_points;

        Utility.Extra_Mesh.Update_Mesh(ref m_feedback_mesh, m_feedback_verts, m_triangles);
        Utility.Extra_Mesh.Update_Mesh(ref m_mesh, m_verts, m_triangles);
    }

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
        m_to_player_direction = Set_Direction_To_Target(m_entity.transform.position, p_target);
        m_to_player_distance = Set_Distance_To_Target(m_entity.transform.position, p_target);

        Update_Trigger_Zone(p_view_direction);

        if (in_trigger_zone) // Only Raycast if player is in the trigger zone
        {
            var hit = Physics2D.Raycast(m_entity.transform.position, m_to_player_direction * m_to_player_distance);
            if (hit.collider.CompareTag(p_target.tag))
            {
                m_feedback_Factor += Time.deltaTime * m_sense_speed;
                Utility.Extra_Math.Interpolate(ref m_feedback_Factor);
                return true;
            }
            else
                m_feedback_Factor = 0.0f;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                m_feedback_Factor = 0;
                in_trigger_zone = true;

                break;

            case "lightCollider":
                StartCoroutine(fade_in_color());
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "lightCollider":
                if (!fade_out_running && !fade_in_running && m_mesh_renderer.material.color.a != 1)
                    StartCoroutine(fade_in_color());
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                m_feedback_Factor = 0;
                in_trigger_zone = false;

                break;
            case "lightCollider":
                if (gameObject.activeSelf)
                    StartCoroutine(fade_out_color());
                break;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    bool fade_out_running;
    IEnumerator fade_out_color()
    {
        float factor = 1;
        while (factor > 0)
        {
            fade_out_running = true;
            factor -= Time.deltaTime * 8.0f;
            Utility.Extra_Math.Interpolate(ref factor);

            m_feedback_mesh_renderer.material.color = (fill_feedback * factor);
            m_mesh_renderer.material.color = (base_feedback * factor);
            yield return new WaitForEndOfFrame();
        }
        //m_mesh_renderer.material.color = Color.clear;
        fade_out_running = false;
    }

    public void fade_out()
    {
        StartCoroutine(fade_out_color());
    }

    bool fade_in_running;
    IEnumerator fade_in_color()
    {
        float factor = 0;
        while (factor < 1)
        {
            fade_out_running = true;
            factor += Time.deltaTime * 8.0f;
            Utility.Extra_Math.Interpolate(ref factor);

            m_feedback_mesh_renderer.material.color = (fill_feedback * factor);
            m_mesh_renderer.material.color = (base_feedback * factor);
            yield return new WaitForEndOfFrame();
        }
        fade_in_running = false;
    }
}
