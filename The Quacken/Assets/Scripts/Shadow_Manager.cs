using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shadow_Manager : MonoBehaviour
{
    public CompositeCollider2D m_objects_composite_collider;

    Vector2[][] m_paths_points;

    Vector3[] m_ray_hit_points;
    Vector3[] m_all_points;

    private GameObject m_player;

    public Material m_test_mat;
    MeshFilter m_mesh_filter;
    MeshRenderer m_mesh_renderer;
    Mesh m_mesh;
    GameObject m_mesh_object;
    PolygonCollider2D m_mesh_collider;
    GameObject m_mesh_collider_object;
    int[] m_indeces;

    public Camera camera;
    Vector2[] m_bound_points = new Vector2[4];

    private void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
        Get_Points();
        Cast_Rays();
        Set_Indeces();
        Create_Mesh();
    }

    private void Update()
    {
        Get_Points();
        Cast_Rays();
        Set_Indeces();
        Update_Mesh();


        Debug.DrawLine(m_player.transform.position, m_bound_points[0], Color.green);
        Debug.DrawLine(m_player.transform.position, m_bound_points[1], Color.green);
        Debug.DrawLine(m_player.transform.position, m_bound_points[2], Color.green);
        Debug.DrawLine(m_player.transform.position, m_bound_points[3], Color.green);
    }

    void Create_Mesh()
    {
        m_mesh_object = new GameObject("m_shadow");
        m_mesh_object.transform.parent = transform;
        m_mesh_collider_object = new GameObject("s_shadow_collider");
        m_mesh_collider_object.transform.parent = m_mesh_object.transform;
        Rigidbody2D temp = m_mesh_collider_object.AddComponent<Rigidbody2D>();
        temp.bodyType = RigidbodyType2D.Static;

        m_mesh_object.layer = LayerMask.NameToLayer("Light_Overlay");
        m_mesh_collider_object.layer = LayerMask.NameToLayer("Ignore Raycast");

        m_mesh_object.transform.position = Vector3.back;

        m_mesh = new Mesh();
        m_mesh_renderer = m_mesh_object.AddComponent<MeshRenderer>();
        m_mesh_filter = m_mesh_object.AddComponent<MeshFilter>();

        m_mesh_collider = m_mesh_collider_object.AddComponent<PolygonCollider2D>();
        m_mesh_collider.isTrigger = true;

        m_mesh_filter.sharedMesh = m_mesh;

        m_mesh_collider.points = m_ray_hit_points.To_Vector2_Array();
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
  
    void Get_Points()
    {
        m_all_points = new Vector3[((m_objects_composite_collider.pointCount) * 3) + 4];
        int point_index = 0;

        m_paths_points = new Vector2[m_objects_composite_collider.pathCount][];

        // Points in Scene
        int j = 0;
        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            m_paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, m_paths_points[i]);

            for (j = point_index; j < point_index + nrOfPoints; j++)
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

        // Camera Bounds
        m_bound_points[0] = new Vector2((m_player.transform.position.x + ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y + ((180.0f / (2.0f * 16.0f)))));
        m_bound_points[1] = new Vector2((m_player.transform.position.x - ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y - ((180.0f / (2.0f * 16.0f)))));
        m_bound_points[2] = new Vector2((m_player.transform.position.x + ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y - ((180.0f / (2.0f * 16.0f)))));
        m_bound_points[3] = new Vector2((m_player.transform.position.x - ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y + ((180.0f / (2.0f * 16.0f)))));

        for (int index = 0; index < m_bound_points.Length; index++)
        {
            m_all_points[j * 3] = m_bound_points[index];
        }

        Quick_Sort(m_all_points, 1, m_all_points.Length - 1, m_player.transform.position);
    }

    List<Vector3> m_ray_points_list = new List<Vector3>();
    void Cast_Rays()
    {
        m_ray_points_list.Clear();


        //m_ray_hit_points = new Vector3[m_all_points.Length + 1];
        //m_ray_hit_points[0] = m_player.transform.position;
        m_ray_points_list.Add(m_player.transform.position);
        for (int i = 0; i < m_all_points.Length; i++)
        {
            //if(Math.Abs(Math.Atan2(m_all_points[i].x - m_player.transform.position.x, m_all_points[i].y - m_player.transform.position.y)) <= 30.0)

            //if (Vector2.Distance(m_player.transform.position, m_all_points[i]) <= Vector2.Distance(m_player.transform.position, m_bound_points[0]))
            //{
                RaycastHit2D hit = Physics2D.Raycast(m_player.transform.position, (m_all_points[i] - m_player.transform.position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid"));
                if (hit.collider != null)
                    m_ray_points_list.Add(hit.centroid);
                else
                    m_ray_points_list.Add(m_all_points[i]);
                //RaycastHit2D hit = Physics2D.Raycast(m_player.transform.position, (m_all_points[i] - m_player.transform.position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid"));
                //if (hit.collider != null)
                //    m_ray_hit_points[i + 1] = hit.centroid;
                //else
                //    m_ray_hit_points[i + 1] = m_all_points[i];
            //}
        }
        
        //m_ray_hit_points = m_ray_hit_points.OrderByDescending(v => Mathf.Atan2(v.x - transform.position.x, v.y - transform.position.y)).ToArray<Vector3>();
        m_ray_hit_points = m_ray_points_list.ToArray();
        Quick_Sort(m_ray_hit_points, 1, m_ray_points_list.Count - 1, m_player.transform.position);
    }
    void Set_Indeces()
    {
        m_indeces = new int[(m_ray_hit_points.Length - 1) * 3];
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

    private static double pivot = new double();
    private static double angle = new double();
    private static Vector3 temp1 = new Vector3();
    private static Vector3 temp2 = new Vector3();
    static public int Partition(Vector3[] arr, int low, int high, Vector3 origin)
    {
        //float pivot = Vector2.SignedAngle(Vector3.up, (arr[high] - origin));
        pivot = Math.Atan2(arr[high].x - origin.x, arr[high].y - origin.y);

        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            //float angle = Vector2.SignedAngle(Vector3.up, (arr[j] - origin));
            angle = Math.Atan2(arr[j].x - origin.x, arr[j].y - origin.y);
            if (angle < pivot)
            {
                i++;
                temp1 = arr[i];
                arr[i] = arr[j];
                arr[j] = temp1;
            }
        }
        temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1;
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
