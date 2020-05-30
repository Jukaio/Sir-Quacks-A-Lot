using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using CSKicksCollection.Trees;
using Utility;

[RequireComponent(typeof(Shadow_Renderer))]
public class Shadow : MonoBehaviour
{
    // Renders the Shadow_Mesh
    private Shadow_Renderer m_shadow_renderer;

    // Camera with bounds
    public UnityEngine.U2D.PixelPerfectCamera m_camera;
    private Rect m_camera_bounds;


    // The grid edge data
    public CompositeCollider2D m_map;
    public CompositeCollider2D Map
    {
        get => m_map;
        set => m_map = value;
    }
    public void Set_Map(GameObject p_game_object)
    {
        Map = p_game_object.GetComponent<CompositeCollider2D>();
    }
    public void Set_Map(CompositeCollider2D p_collider)
    {
        Map = p_collider;
    }

    // Private containers for RayHitPoints and Triangles
    private List<Vector3> m_hit_points = new List<Vector3>();

    // ListToArray() container
    private List<int> m_triangles = new List<int>();

    // List of all the segments
    List<List<Segment>> m_segments = new List<List<Segment>>(); // Exchange with Binary tree (O(n) vs (O(log(n))

    // Visible points
    List<Point> m_points = new List<Point>();

    void Paths_To_Segments()
    {
        // Clear the segment list (List of Segments = Polygon)
        foreach (List<Segment> list in m_segments)
            list.Clear();
        m_segments.Clear();
        m_points.Clear();


        Vector2 map_position = m_map.transform.position;

        // Jagged array of points of each path (Empty)
        Vector2[][] paths_points = new Vector2[m_map.pathCount][];

        for (int i = 0; i < m_map.pathCount; i++)
        {
            // Get Path with points
            // Get the points of path i
            paths_points[i] = new Vector2[m_map.GetPathPointCount(i)];

            // Get amount of points for offset 
            int amount_of_points = m_map.GetPath(i, paths_points[i]);

            // Small short cut for the path points
            Vector2[] path = paths_points[i];

            List<Segment> polygon = new List<Segment>();

            // ID of segment
            int segment_id = 0;
            // Amount of points added during the current polygon
            int points_added = 0;
            // Go through each point in path i

            List<Point> points = new List<Point>();
            for (int j = 0; j < amount_of_points; j++)
            {
                // Segment coordinates
                int start_index = j;
                // if the end point is out of bounds, use point 0 of path as end for last segment
                int end_index_right = (j + 1 < path.Length) ? j + 1 : 0;

                // Create a Segment with the points of the current edge/path partition 
                Vector2 start_vector = path[start_index] + map_position; // Start Vert
                Vector2 end_vector = path[end_index_right] + map_position; // End Vert
                Segment segment = new Segment(start_vector, end_vector, i, segment_id);

                // If the segment is somewhat within the bounds - Use it! (The points are in the bounds or the line intersects with the camera bounds)
                if (Extra_Collision.Rectangle_Line_Collison(segment, m_camera_bounds) || 
                    Extra_Collision.In_Bounds(start_vector, m_camera_bounds) || 
                    Extra_Collision.In_Bounds(end_vector, m_camera_bounds))
                {
                    // Create start and end points based on the continous data for a polygon and segment
                    Point start = new Point(start_vector, segment_id, -1, i);
                    Point end = new Point(end_vector, segment_id, -1, i);
                    // If a point already exists, refresh the data of it
                    // Else just add it and advance the points_added counter

                    int s = points.IndexOf(start);
                    if (s > -1)
                    {
                        var temp = points[s];
                        temp.second_segment = segment_id;
                        points[s] = temp;
                    }
                    else
                    {
                        points.Add(start);
                        points_added++;
                    }

                    int e = points.IndexOf(end);
                    if (e > -1)
                    {
                        var temp = points[e];
                        temp.second_segment = segment_id;
                        points[e] = temp;
                    }
                    else
                    {
                        points.Add(end);
                        points_added++;
                    }

                    // Add the segment to the list
                    // Advance the segment_id by one
                    // 
                    segment.m_id = segment_id;
                    polygon.Add(segment); 
                    segment_id++;
                }
            }
            m_points.AddRange(points);
            m_segments.Add(polygon);
        }
        
        // Sort the points by angle
        m_points = m_points.OrderBy(p => Angle.Angle_Between_Segments(p.m_coordinate, Vector2.left + (transform.position * Vector2.up), transform.position)).ToList();
    }

    private void Start()
    {
        m_shadow_renderer = GetComponent<Shadow_Renderer>();
        StartCoroutine(Update_Shadow());
    }

    IEnumerator Update_Shadow()
    {
        while (true)
        {
            float before = Time.realtimeSinceStartup;
            // Just a quick shortcut for the player position
            Vector2 position = transform.position;

            // Get the camera bounds with an offset size
            m_camera_bounds = Utility.Bounds.Create_Camera_Rect(m_camera, m_camera.transform.position, 2.0f);

            // Make Segments out of the Paths
            Paths_To_Segments();

            m_points = Visibility.Find_Visible_Points(m_segments, ref m_points, position);

            m_triangles.Clear();
            m_hit_points.Clear();

            Vector2[] bounds = { m_camera_bounds.min + Vector2.down  + Vector2.left,
                             m_camera_bounds.max + Vector2.right + Vector2.up,
                             new Vector2(m_camera_bounds.xMin - 1, m_camera_bounds.yMax + 1),
                             new Vector2(m_camera_bounds.xMax + 1, m_camera_bounds.yMin - 1)};

            int added = 1; // First non-player position vert
            foreach (var bound in bounds)
            {
                Vector2 temp = bound - position;
                float length = Mathf.Sqrt((temp.x * temp.x) + (temp.y * temp.y));
                var hit = Physics2D.Raycast(position, bound - position, length, 1 << LayerMask.NameToLayer("Grid"));
                if (hit.collider != null)
                    m_hit_points.Add(hit.point);
                else
                    m_hit_points.Add(bound);

                m_triangles.Add(0);         // Origin vert for triangle
                m_triangles.Add(added);     // Left vert for triangle
                m_triangles.Add(added + 1); // Right vert for triangle
                added++;
            }

            foreach (Point point in m_points)
            {
                for (int i = -1; i < 2; i++)
                {
                    Vector2 hit_point = Physics2D.Raycast(position, (point.m_coordinate.Rotate(0.004f * i) - position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid")).point;
                    m_hit_points.Add(hit_point);
                    m_triangles.Add(0);         // Origin vert for triangle
                    m_triangles.Add(added);     // Left vert for triangle
                    m_triangles.Add(added + 1); // Right vert for triangle
                    added++;
                }
            }
            m_triangles[m_triangles.Count - 1] = 1; // Correct the last triangle


            // Sorts from index 1 to the end of the array (skips the player position
            // Player position is always indexed as 0;
            m_hit_points = m_hit_points.OrderBy(p => Angle.Angle_Between_Segments(p, Vector2.left, transform.position)).ToList();
            m_hit_points.Insert(0, position);

            m_shadow_renderer.Triangles = m_triangles.ToArray();
            m_shadow_renderer.Vertices = m_hit_points.ToArray();

            float now = Time.realtimeSinceStartup - before;
            yield return new WaitForSecondsRealtime((1.0f/60.0f) - now);
        }
    }

}
