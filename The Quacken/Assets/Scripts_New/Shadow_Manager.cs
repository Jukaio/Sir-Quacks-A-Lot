using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

public class Shadow_Manager : MonoBehaviour
{
    public CompositeCollider2D m_objects_composite_collider;

    Vector3[] m_ray_hit_points;
    Vector3[] m_corners;

    private GameObject m_player;
    public Material m_shadow_material;
    Mesh m_mesh;
    GameObject m_mesh_object;
    PolygonCollider2D m_mesh_collider;

    int[] m_indeces;

    public Camera camera;
    Vector2[] m_bound_points = new Vector2[4];

    private void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
        Get_Corners();
        Cast_Rays();
        Set_Indeces();
        Create_Mesh();
    }

    private void Update()
    {
        Get_Corners(); // O(n)
        Cast_Rays(); // O(?)
        Set_Indeces(); // 0(n*3)
        Update_Mesh(); 

        //Debug.DrawLine(m_player.transform.position, m_bound_points[0], Color.green);
        //Debug.DrawLine(m_player.transform.position, m_bound_points[1], Color.green);
        //Debug.DrawLine(m_player.transform.position, m_bound_points[2], Color.green);
        //Debug.DrawLine(m_player.transform.position, m_bound_points[3], Color.green);

        foreach (Vector2 ray in m_corners)
            Debug.DrawLine(m_player.transform.position, ray, Color.blue);
    }

    void Create_Mesh()
    {
        m_mesh = Extra_Mesh.Create_Mesh(out m_mesh_object, out m_mesh_collider,
                                    "m_shadow", "m_shadow_collider",
                                    "Light_Overlay", "Ignore Raycast",
                                    new Material(m_shadow_material));
 
    }
    void Update_Mesh()
    {
        m_mesh_collider.points = m_ray_hit_points.To_Vector2_Array();
        Extra_Mesh.Update_Mesh(ref m_mesh, m_ray_hit_points, m_indeces);
    }

    List<Vector3> m_corner_list = new List<Vector3>();
    void Get_Corners()
    {
        m_corner_list.Clear();
        m_corners = new Vector3[((m_objects_composite_collider.pointCount)) * 3];
        int point_index = 0;

        Vector2[][] paths_points = new Vector2[m_objects_composite_collider.pathCount][];

        // Points in Scene
        int j = 0;
        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, paths_points[i]);

            for (j = point_index; j < point_index + nrOfPoints; j++)
            {
                // Point
                Vector2 temp = paths_points[i][j - point_index];
                m_corners[j * 3] = temp;

                // Offsets
                m_corners[j * 3 + 1] = temp.Rotate(-0.02f);
                m_corners[j * 3 + 2] = temp.Rotate(0.02f);
            }
            point_index += nrOfPoints;
        }

        //// Camera Bounds
        //m_bound_points[0] = new Vector2((m_player.transform.position.x + ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y + ((180.0f / (2.0f * 16.0f)))));
        //m_bound_points[1] = new Vector2((m_player.transform.position.x - ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y - ((180.0f / (2.0f * 16.0f)))));
        //m_bound_points[2] = new Vector2((m_player.transform.position.x + ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y - ((180.0f / (2.0f * 16.0f)))));
        //m_bound_points[3] = new Vector2((m_player.transform.position.x - ((320.0f / (2.0f * 16.0f)))), (m_player.transform.position.y + ((180.0f / (2.0f * 16.0f)))));

        //for (int index = 0; index < m_bound_points.Length; index++)
        //{
        //    m_corners[j * 3] = m_bound_points[index];
        //}
        
        Utility.Sort_Vectors_By.Angle.Quick_Sort(m_corners, 1, m_corners.Length - 1, m_player.transform.position);
    }

    List<Vector3> m_ray_points_list = new List<Vector3>();
    void Cast_Rays()
    {
        m_ray_points_list.Clear();

        m_ray_points_list.Add(m_player.transform.position);
        for (int i = 0; i < m_corners.Length; i++)
        {

            RaycastHit2D hit = Physics2D.Raycast(m_player.transform.position, (m_corners[i] - m_player.transform.position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid"));
            if (hit.collider != null)
                m_ray_points_list.Add(hit.point);
            else
                m_ray_points_list.Add(m_corners[i]);

        }
        
        m_ray_hit_points = m_ray_points_list.ToArray();
        Utility.Sort_Vectors_By.Angle.Quick_Sort(m_ray_hit_points, 1, m_ray_points_list.Count - 1, m_player.transform.position);
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
}
