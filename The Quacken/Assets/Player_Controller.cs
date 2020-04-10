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

    Vector3[] m_ray_hit_points;
    Vector3[] m_all_points;
    public Player_Input m_input;

    Ray[] rays;

    public Material m_test_mat;
    MeshFilter m_mesh_filter;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;
    GameObject m_mesh_object;
    PolygonCollider2D m_mesh_collider;
    int[] m_indeces;

    private void Awake()
    {

    }

    void Start()
    {
        Service<Game_Manager>.Get().Set_Player(gameObject);
        m_input = Player_Input.Player(0);
        Get_Corners();
        Cast_Rays();
        Set_Indeces();
        Create_Mesh();

    }

    void Update_Noise_Range()
    {
        transform.GetChild(1).transform.localScale = (Vector3.one / 10.0f) * m_noise_range;

    }

    public UnityEngine.UI.Text text;
    float dt = 0.0f;
    float FPS = 0.0f;

    void Update()
    {
        dt += Time.deltaTime;
        dt /= 2.0f;
        FPS = 1.0f / dt;

        text.text = FPS.ToString();

        Update_Noise_Range();

        if (m_input.Move_Left)
            transform.position += Vector3.left * Time.deltaTime * 5.0f;
        if (m_input.Move_Right)
            transform.position += Vector3.right * Time.deltaTime * 5.0f;
        if (m_input.Move_Up)
            transform.position += Vector3.up * Time.deltaTime * 5.0f;
        if (m_input.Move_Down)
            transform.position += Vector3.down * Time.deltaTime * 5.0f;
        ////print(Player_Input.Player(0).m_current_device);

        Cast_Rays();
        Update_Mesh();

    }

    void Get_Corners()
    {
        m_all_points = new Vector3[m_objects_composite_collider.pointCount * 3];
        int point_index = 0;

        m_paths_points = new Vector2[m_objects_composite_collider.pathCount][];

        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            m_paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, m_paths_points[i]);

            for (int j = point_index; j < point_index + nrOfPoints; j++)
            {
                // Point
                Vector2 temp = m_paths_points[i][j - point_index];
                m_all_points[j * 3] = temp;

                // Offsets
                m_all_points[j * 3 + 1] = temp.Rotate(-0.02f);
                m_all_points[j * 3 + 2] = temp.Rotate(0.02f);
            }
            point_index += nrOfPoints;
        }
    }


    void Cast_Rays()
    {
        m_ray_hit_points = new Vector3[m_all_points.Length + 1];
        m_ray_hit_points[0] = transform.position;

        for (int i = 0; i < m_all_points.Length; i++)
        {

            m_ray_hit_points[i + 1] = Physics2D.Raycast(transform.position, (m_all_points[i] - transform.position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid")).centroid;
        }

        Quick_Sort(m_ray_hit_points, 1, m_ray_hit_points.Length - 1, transform.position);
    }

    GameObject mesh_collider;
    void Create_Mesh()
    {
        m_mesh_object = new GameObject("Mesh Visual");
        mesh_collider = new GameObject("collider");
        mesh_collider.transform.parent = m_mesh_object.transform;

        m_mesh_object.layer = LayerMask.NameToLayer("Lighting");
        mesh_collider.layer = LayerMask.NameToLayer("Ignore Raycast");

        m_mesh_object.transform.position = Vector3.back;
        

        m_mesh = new Mesh();
        m_mesh_renderer = m_mesh_object.AddComponent<MeshRenderer>();
        m_mesh_filter = m_mesh_object.AddComponent<MeshFilter>();

        m_mesh_collider = mesh_collider.AddComponent<PolygonCollider2D>();

        m_mesh_filter.sharedMesh = m_mesh;


        m_mesh_collider.points =  m_ray_hit_points.To_Vector2_Array();
        m_mesh.vertices = m_ray_hit_points;
        m_mesh.triangles = m_indeces;

        Material material = new Material(m_test_mat);
        m_mesh_renderer.material = material;

    }

    void Update_Mesh()
    {
        m_mesh.Clear();
        m_mesh_collider.points = m_ray_hit_points.To_Vector2_Array();
        m_mesh.vertices = m_ray_hit_points;
        m_mesh.triangles = m_indeces;
    }

    void Set_Indeces()
    {
        m_indeces = new int[(m_all_points.Length) * 3];
        int counter = 1;
        int index = 0;
        for (; index < m_indeces.Length - 3; index += 3)
        {
            m_indeces[index] = 0;
            m_indeces[index + 1] = counter;
            m_indeces[index + 2] = counter + 1;
            counter++;
        }
        m_indeces[index] = 0;
        m_indeces[index + 1] = counter;
        m_indeces[index + 2] = 1;

    }

    public static object Binary_Search(Vector3[] arr, float key)
    {
        int min = 0;
        int max = arr.Length - 1;

        while(min <= max)
        {
            int mid = (min + max) / 2;
            if(key == Vector2.SignedAngle(Vector3.up, arr[mid]))
            {
                return ++mid;
            }
            else if(key < Vector2.SignedAngle(Vector3.up, arr[mid]))
            {
                max = mid - 1;
            }
            else
            {
                min = mid + 1;
            }
        }
        return null;
    }

    static public int Partition(Vector3[] arr, int low, int high, Vector3 origin)
    {
        float pivot;
        pivot = Vector2.SignedAngle(Vector3.up, (arr[high] - origin).normalized);

        int i = (low - 1);
        {
            for (int j = low; j < high; j++)
            {
                float angle = Vector2.SignedAngle(Vector3.up, (arr[j] - origin).normalized);
                if (angle < pivot)
                {
                    i++;
                    Vector3 temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            Vector3 temp1 = arr[i+1];
            arr[i+1] = arr[high];
            arr[high] = temp1;

            return i + 1;
        }
    }


    public static void Quick_Sort(Vector3[] arr, int low, int high, Vector3 origin)
    {
        if (low < high)
        {
            int pivot = Partition(arr, low, high, origin);
            Quick_Sort(arr, low, pivot - 1, origin);
            Quick_Sort(arr, pivot + 1, high, origin);
        }
    }
}

