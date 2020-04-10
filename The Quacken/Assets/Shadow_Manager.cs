using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Shadow
{
    public Shadow(string p_name, Material p_material, Transform p_parent)
    {
        m_mesh_object = new GameObject(p_name);
        m_mesh_object.layer = LayerMask.NameToLayer("Lighting");
        m_mesh_object.transform.position = Vector3.back;
        m_mesh_object.transform.parent = p_parent;

        m_mesh = new Mesh();
        m_renderer = m_mesh_object.AddComponent<MeshRenderer>();
        m_filter = m_mesh_object.AddComponent<MeshFilter>();

        m_filter.sharedMesh = m_mesh;

        m_mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
        indexes = new int[] { 0, 1, 2 };
        m_mesh.triangles = indexes; ;

        Material material = new Material(p_material);
        m_renderer.material = material;
    }

    public void Set_Vertices(Vector3[] p_verts)
    {
        m_mesh.Clear();
        m_mesh.vertices = p_verts;
        m_mesh.triangles = indexes;
    }

    GameObject m_mesh_object;
    Mesh m_mesh;
    MeshRenderer m_renderer;
    MeshFilter m_filter;
    int[] indexes;
}

public class Shadow_Manager : MonoBehaviour
{
    GameObject m_player;

    Shadow[] m_shadows;
    public Material m_material;
    public CompositeCollider2D m_objects_composite_collider;

    private Vector3[] m_all_corners;
    private Vector3[] m_mesh_points;
    void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
        Get_Corners();

    }

    void Update()
    {
        Cast_Rays();
    }

    void Get_Corners()
    {
        Vector2[][] paths_points = new Vector2[m_objects_composite_collider.pathCount][];
        m_all_corners = new Vector3[m_objects_composite_collider.pointCount * 3];

        int point_index = 0;

        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, paths_points[i]);

            for (int j = point_index; j < point_index + nrOfPoints; j++)
            {
                // Point
                Vector2 temp = paths_points[i][j - point_index];
                m_all_corners[j * 3] = temp;

                // Offsets
                m_all_corners[j * 3 + 1] = temp.Rotate(-0.02f);
                m_all_corners[j * 3 + 2] = temp.Rotate(0.02f);
            }
            point_index += nrOfPoints;
        }
    }

   
    void Cast_Rays()
    {
        m_mesh_points = new Vector3[m_all_corners.Length + 1];
        m_mesh_points[0] = m_player.transform.position;

        //Vector3[] temp = new Vector3[m_all_corners.Length + 1];
        for (int i = 0; i < m_all_corners.Length; i++)
        {
            m_mesh_points[i + 1] = Physics2D.Raycast(transform.position, (m_all_corners[i] - m_mesh_points[0])).centroid;
            //m_mesh_points[i + 1] = Vector3.one * Random.Range(0.1f, 3.0f);
        }
        Quick_Sort(m_mesh_points, 1, m_mesh_points.Length - 1);
    }

    static public int Partition(Vector3[] arr, int left, int right)
    {
        float pivot = Vector2.SignedAngle(Vector3.up, (arr[left] - arr[0]));

        int i = (left - 1);
        {
            for (int j = left; j < right; j++)
            {
                float angle = Vector2.SignedAngle(Vector3.up, (arr[j] - arr[0]));
                if (angle < pivot)
                {
                    i++;
                    Vector3 temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            Vector3 temp1 = arr[i + 1];
            arr[i + 1] = arr[right];
            arr[right] = temp1;

            return i + 1;
        }
    }

    public static void Quick_Sort(Vector3[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);

            if (pivot > 1)
                Quick_Sort(arr, left, pivot - 1);

            if (pivot + 1 < right)
                Quick_Sort(arr, pivot + 1, right);
        }
    }
}
