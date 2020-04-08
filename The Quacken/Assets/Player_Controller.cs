using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[System.Serializable]
public struct Edge
{
    public Edge(Vector2 p_start, Vector2 p_end)
    {
        m_start = p_start;
        m_end = p_end;
    }

    public Vector2 m_start;
    public Vector2 m_end;
}

public class Player_Controller : MonoBehaviour
{
    public float m_noise_range = 3.0f;
    public CompositeCollider2D m_objects_composite_collider;

    Vector2[][] m_paths_points;
    Vector3[] m_all_points;
    public Player_Input m_input;



    public Material m_test_mat;
    MeshFilter m_mesh_filter;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;
    GameObject m_mesh_object;
    int[] m_indeces;

    private void Awake()
    {

    }

    void Start()
    {
        Service<Game_Manager>.Get().Set_Player(gameObject);
        m_input = Player_Input.Player(0);
        Get_Corners();
    }

    void Update_Noise_Range()
    {
        transform.GetChild(1).transform.localScale = (Vector3.one / 10.0f) * m_noise_range;

    }

    void Update()
    {
        Update_Noise_Range();

        //Set_Ray_Data();
        Cast_Rays();


        if (m_input.Move_Left)
            transform.position += Vector3.left * Time.deltaTime * 5.0f;
        if (m_input.Move_Right)
            transform.position += Vector3.right * Time.deltaTime * 5.0f;
        if (m_input.Move_Up)
            transform.position += Vector3.up * Time.deltaTime * 5.0f;
        if (m_input.Move_Down)
            transform.position += Vector3.down * Time.deltaTime * 5.0f;
        ////print(Player_Input.Player(0).m_current_device);
    }

    void Get_Corners()
    {
        m_all_points = new Vector3[m_objects_composite_collider.pointCount + 1];
        m_all_points[0] = transform.position;
        int point_index = 1;

        m_paths_points = new Vector2[m_objects_composite_collider.pathCount][];

        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            m_paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, m_paths_points[i]);

            for (int j = point_index; j < point_index + nrOfPoints; j++)
                m_all_points[j] = m_paths_points[i][j - point_index];
            point_index += nrOfPoints;
        }
    }

    void Cast_Rays()
    {
        foreach (Vector3 point in m_all_points)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, point - transform.position);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
            }

            Debug.DrawRay(transform.position, point - transform.position, Color.green);
        }
    }

    void Debug_Draw_Rays()
    {
        for (int arr_i = 0; arr_i < m_paths_points.Length; arr_i++)
        {
            for (int jag_i = 0; jag_i < m_paths_points[arr_i].Length; jag_i++)
            {
                Debug.DrawRay(transform.position, (Vector3)m_paths_points[arr_i][jag_i] - transform.position, Color.green);
            }
        }
    }

    void Create_Mesh()
    {
        //m_mesh_object = new GameObject("Mesh Visual");
        //m_mesh_object.transform.position = Vector3.zero;

        //m_mesh = new Mesh();
        //m_mesh_renderer = m_mesh_object.AddComponent<MeshRenderer>();
        //m_mesh_filter = m_mesh_object.AddComponent<MeshFilter>();

        //m_mesh_filter.sharedMesh = m_mesh;

        //m_mesh.vertices = m_all_points;
        //m_mesh.triangles = m_indeces;

        //Material material = new Material(m_test_mat);

        //m_mesh_renderer.material = material;
    }

    void Set_Indeces()
    {
        //m_indeces = new int[(m_all_points.Length - 1) * 3];
        //int counter = 1;
        //int index = 0;
        //for (; index < m_indeces.Length - 3; index += 3)
        //{
        //    m_indeces[index] = 0;
        //    m_indeces[index + 1] = counter;
        //    m_indeces[index + 2] = counter + 1;
        //    counter++;
        //}
        //m_indeces[index] = 0;
        //m_indeces[index + 1] = counter;
        //m_indeces[index + 2] = 1;
    }
}
