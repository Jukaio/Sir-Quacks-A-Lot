using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using CSKicksCollection.Trees;


[RequireComponent(typeof(Shadow_Renderer))]
public class Shadow : MonoBehaviour
{
    Shadow_Renderer m_shadow_Renderer;

    public struct Point 
    {
        public Point(Vector2 p_point)
        {
            m_coordinate = p_point;
            first_segment = -1;
            second_segment = -1;
            m_polygon_id = -1;
        }

        public Point(Vector2 p_point, int p_first_segment, int p_second_segment, int p_polygon_id)
        {
            m_coordinate = p_point;
            first_segment = p_first_segment;
            second_segment = p_second_segment;
            m_polygon_id = p_polygon_id;
        }

        public Vector2 m_coordinate;

        public int m_polygon_id;
        public int first_segment;
        public int second_segment;
    }
    struct Segment : IComparable // Could add a function to swap start and end according to Atan2
    {
        public Segment(Vector2 p_start, Vector2 p_end)
        {
            m_start = p_start;
            m_end = p_end;
            m_polygon_id = -1;
            m_id = -1;
        }

        public Segment(Vector2 p_start, Vector2 p_end, int p_polygon_id, int p_id)
        {
            m_start = p_start;
            m_end = p_end;
            m_polygon_id = p_polygon_id;
            m_id = p_id;
        }

        public Vector2 m_start;
        public Vector2 m_end;
        public int m_polygon_id;
        public int m_id;

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    // Camera with bounds
    public UnityEngine.U2D.PixelPerfectCamera m_camera;

    // The grid edge data
    public CompositeCollider2D m_collider;

    // Private containers for RayHitPoints and Triangles
    private List<Vector3> m_hit_points = new List<Vector3>();

    // ListToArray() container
    private List<int> m_triangles = new List<int>();

    // List of all the segments
    List<List<Segment>> m_segments = new List<List<Segment>>();

    // Visible points
    List<Point> m_points = new List<Point>();

    void Paths_To_Segments()
    {
        // Camera rectangle
        Rect camera_bounds = Create_Camera_Rect();

        // Clear the segment list (List of Segments = Polygon)
        foreach (List<Segment> list in m_segments)
            list.Clear();
        m_segments.Clear();

        m_points.Clear();

        // Jagged array of points of each path (Empty)
        Vector2[][] paths_points = new Vector2[m_collider.pathCount][];

        for (int i = 0; i < m_collider.pathCount; i++)
        {
            // Get Path with points
            // Get the points of path i
            paths_points[i] = new Vector2[m_collider.GetPathPointCount(i)];

            // Get amount of points for offset 
            int amount_of_points = m_collider.GetPath(i, paths_points[i]);
            // Small short cut for the path points
            Vector2[] path = paths_points[i];

            List<Segment> polygon = new List<Segment>();
            Segment prev_segment = new Segment();

            int segment_id = 0;

            // Go through each point in path i
            for (int j = 0; j < amount_of_points; j++)
            {
                // Segment_Id in Polygon

                // Segment coordinates
                int start_index = j;
                // if the end point is out of bounds, use point 0 of path as end for last segment
                int end_index_right = (j + 1 < path.Length) ? j + 1 : 0;

                Vector2 start_vector = path[start_index]; // Start Vert
                Vector2 end_vector = path[end_index_right]; // End Vert

                Segment segment = new Segment(start_vector, end_vector, i, segment_id);
 
                if (Rectangle_Line_Collison(segment, camera_bounds) || In_Bounds(start_vector, camera_bounds) || In_Bounds(end_vector, camera_bounds))
                {
                    if ((prev_segment.m_end != segment.m_start &&
                         prev_segment.m_start != segment.m_end &&
                         prev_segment.m_end != segment.m_end &&
                         prev_segment.m_start != segment.m_start) || segment_id == 0)
                    {
                        Point start = new Point(start_vector, segment_id, -1, i);
                        Point end = new Point(end_vector, segment_id, -1, i);

                        m_points.Add(start);
                        m_points.Add(end);
                    }
                    else if (end_index_right == 0)
                    {
                        Point start = new Point(prev_segment.m_end, prev_segment.m_id, segment_id, i);
                        start.second_segment = segment_id;

                        Point end = new Point(end_vector, m_points[0].first_segment, segment_id, i);

                        m_points[m_points.Count - 1] = start;
                        m_points[m_points.Count - segment_id - 1] = end;
                    }
                    else
                    {
                        Point start = new Point(prev_segment.m_end, prev_segment.m_id, segment_id, i);
                        start.second_segment = segment_id;

                        Point end = new Point(end_vector, segment_id, -1, i);
                        m_points[m_points.Count - 1] = start;
                        m_points.Add(end);
                    }

                    polygon.Add(segment); // Add Segment to list
                    segment_id++;

                    prev_segment = segment;
                }
            }
            m_segments.Add(polygon);
        }

        // Sort points
        // Distinct with a for loop
        // For
        // if a point is doubled -> Merge them
        // In sorted list the points follow one by one
        // A 2D point can not have more than 2 connections
        m_points = m_points.OrderBy(p => Math.Atan2(p.m_coordinate.y - transform.position.y, p.m_coordinate.x - transform.position.x)).ToList();
    }

    Rect Create_Camera_Rect()
    {
        float chw = (m_camera.refResolutionX / m_camera.assetsPPU) / 2.0f; // Camera half width
        float chh = (m_camera.refResolutionY / m_camera.assetsPPU) / 2.0f; // Camera half height

        return Rect.MinMaxRect(transform.position.x - chw, transform.position.y - chh, transform.position.x + chw, transform.position.y + chh);
    }
    bool In_Bounds(Vector2 p_point, Rect p_bounds)
    {
        return ((p_point.x < p_bounds.xMax && p_point.x > p_bounds.xMin) &&
                (p_point.y < p_bounds.yMax && p_point.y > p_bounds.yMin));
    }

    bool Visible(Point p_point, List<Segment> p_edges)
    {
        //if((int)p_point.m_coordinate.x == 15 && (int)p_point.m_coordinate.y == 12)
        //{
        //    Debug.DrawLine(p_point.m_coordinate + Vector2.left, p_point.m_coordinate + Vector2.up, Color.green);
        //    Debug.DrawLine(p_point.m_coordinate + Vector2.right, p_point.m_coordinate + Vector2.up, Color.green);
        //    Debug.DrawLine(p_point.m_coordinate + Vector2.left, p_point.m_coordinate + Vector2.down, Color.green);
        //    Debug.DrawLine(p_point.m_coordinate + Vector2.right, p_point.m_coordinate + Vector2.down, Color.green);
        //}

        foreach(var edge in p_edges)
        {
            if (edge.m_id == p_point.first_segment || edge.m_id == p_point.second_segment)
                continue;

            if (Line_Line_Intersection(edge, new Segment(transform.position, p_point.m_coordinate)))
            {
                return false;
            }
        }

        return true;
    }

    List<Point> Get_Visible_Points()
    {
        Segment halfline = new Segment(transform.position, (2000.0f * Vector2.left) + (transform.position * Vector2.up));
        List<Segment> edges = new List<Segment>();
        List<Point> visible_points = new List<Point>();

        foreach(List<Segment> list in m_segments)
        {
            foreach(Segment line in list)
            {
                if (Line_Line_Intersection(halfline, line))
                {
                    //Debug.DrawLine(line.m_start, line.m_end, Color.green);
                    edges.Add(line);
                }
            }
        }

        for (int i = 0; i < m_points.Count; i++)
        {
            Point point = m_points[i];


            //Debug.DrawLine(transform.position, point.m_coordinate, Color.cyan);

            

            if (Visible(point, edges))
                visible_points.Add(point);

            double main_angle = Angle_Between_Segments(point.m_coordinate, halfline.m_end, transform.position);

            if (point.first_segment > -1)
            {
                var first_segment = m_segments[point.m_polygon_id][point.first_segment];

                Vector2 first_segment_end = (point.m_coordinate != first_segment.m_end) ? first_segment.m_end : first_segment.m_start;
                double first_seg_angle = Angle_Between_Segments(first_segment_end, halfline.m_end, transform.position);

            if (first_seg_angle > main_angle)
                edges.Add(first_segment);
            else
                edges.Remove(first_segment);
            }

            if (point.second_segment > -1)
            {
                var second_segment = m_segments[point.m_polygon_id][point.second_segment];

                Vector2 second_segment_end = (point.m_coordinate != second_segment.m_end) ? second_segment.m_end : second_segment.m_start;
                double second_seg_angle = Angle_Between_Segments(second_segment_end, halfline.m_end, transform.position);

                if (second_seg_angle > main_angle)
                    edges.Add(second_segment);
                else
                    edges.Remove(second_segment);
            }
        }

        return visible_points;
    }

    double Angle_Between_Segments(Segment p_segment_lhs, Segment p_segment_rhs)
    {
        double angle1 = Math.Atan2(p_segment_lhs.m_start.y - p_segment_lhs.m_end.y,
                                   p_segment_lhs.m_start.x - p_segment_lhs.m_end.x);

        double angle2 = Math.Atan2(p_segment_rhs.m_start.y - p_segment_rhs.m_end.y,
                                   p_segment_rhs.m_start.x - p_segment_rhs.m_end.x);

        double temp = angle1 - angle2;
        double angle = temp < 0 ? Math.Abs(temp) + (Math.PI + temp) * 2 : temp;

        return angle;
    }
    double Angle_Between_Segments(Vector2 p_lhs, Vector2 p_rhs, Vector2 p_origin)
    {
        double angle1 = Math.Atan2(p_origin.y - p_lhs.y,
                                   p_origin.x - p_lhs.x);

        double angle2 = Math.Atan2(p_origin.y - p_rhs.y,
                                   p_origin.x - p_rhs.x);

        double temp = angle1 - angle2;
        double angle = temp < 0 ? Math.Abs(temp) + (Math.PI + temp) * 2 : temp;

        return angle;
    }
    bool Polygon_Line_Collision(Segment p_segment, List<Segment> p_verts)
    {
        for(int current = 0; current < p_verts.Count; current++)
        {
            if (Line_Line_Intersection(p_segment, p_verts[current]))
                return true;
        }
        return false;
    }
    bool Polygon_Line_Collision(List<Segment> p_verts, Segment p_segment)
    {
        return Polygon_Line_Collision(p_segment, p_verts);
    }
    bool Rectangle_Line_Collison(Segment p_segment, Rect p_rect)
    {
        float x_min = p_rect.xMin;
        float x_max = p_rect.xMax;
        float y_min = p_rect.yMin;
        float y_max = p_rect.yMax;

        Vector2 top_left = new Vector2(x_min, y_min);
        Vector2 top_right = new Vector2(x_max, y_min);
        Vector2 bottom_left = new Vector2(x_min, y_max);
        Vector2 bottom_right = new Vector2(x_max, y_max);

        if(Line_Line_Intersection(p_segment, new Segment(bottom_left, top_left)))
            return true;
        if(Line_Line_Intersection(p_segment, new Segment(top_right, bottom_right)))
            return true;
        if (Line_Line_Intersection(p_segment, new Segment(top_left, top_right)))
            return true;
        if (Line_Line_Intersection(p_segment, new Segment(bottom_right, bottom_left)))
            return true;

        return false;
    }
    bool Rectangle_Line_Collison(Rect p_rect, Segment p_segment)
    {
        return Rectangle_Line_Collison(p_segment, p_rect);
    }
    bool Line_Line_Intersection(Segment lhs, Segment rhs)
    {
        Vector2 lhs_start = lhs.m_start;
        Vector2 lhs_end = lhs.m_end;
        Vector2 rhs_start = rhs.m_start;
        Vector2 rhs_end = rhs.m_end;

        float uA = (((rhs_end.x - rhs_start.x) * (lhs_start.y - rhs_start.y) - (rhs_end.y - rhs_start.y) * (lhs_start.x - rhs_start.x)) / 
                    ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));
        float uB = (((lhs_end.x - lhs_start.x) * (lhs_start.y - rhs_start.y) - (lhs_end.y - lhs_start.y) * (lhs_start.x - rhs_start.x)) / 
                    ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));

        return ((uA >= 0 && uA <= 1) && (uB >= 0 && uB <= 1));
    }
    bool Line_Line_Intersection(Segment lhs, Segment rhs, ref Vector2 p_intersection)
    {
        Vector2 lhs_start = lhs.m_start;
        Vector2 lhs_end = lhs.m_end;
        Vector2 rhs_start = rhs.m_start;
        Vector2 rhs_end = rhs.m_end;

        float uA = (((rhs_end.x - rhs_start.x) * (lhs_start.y - rhs_start.y) - (rhs_end.y - rhs_start.y) * (lhs_start.x - rhs_start.x)) /
                    ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));
        float uB = (((lhs_end.x - lhs_start.x) * (lhs_start.y - rhs_start.y) - (lhs_end.y - lhs_start.y) * (lhs_start.x - rhs_start.x)) /
                    ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));
        if(((uA >= 0 && uA <= 1) && (uB >= 0 && uB <= 1)))
        {
            p_intersection = lhs_start + (uA * (lhs_end - lhs_start));
            return true;
        }
        return false;
    }

    private void Start()
    {
        m_shadow_Renderer = GetComponent<Shadow_Renderer>();
    }

    void Update()
    {
        // Just a quick shortcut for the player position
        Vector2 position = transform.position;

        foreach (var point in m_points)
        {
            Debug.DrawLine(transform.position, point.m_coordinate, Color.blue);

        }

        // Make Segments out of the Paths
        Paths_To_Segments();
        m_points = Get_Visible_Points();

        m_triangles.Clear();
        m_hit_points.Clear();

        foreach (List<Segment> list in m_segments)
        {
            foreach (Segment segment in list)
            {
                Debug.DrawLine(segment.m_start, segment.m_end, Color.red);
            }
        }



        int added = 1; // First non-player position vert
        foreach (Point point in m_points)
        {
            for (int i = -1; i < 2; i++)
            {
                m_hit_points.Add(Physics2D.Raycast(transform.position, (point.m_coordinate.Rotate(0.002f * i) - position), Mathf.Infinity, 1 << LayerMask.NameToLayer("Grid")).point);
                m_triangles.Add(0);         // Origin vert for triangle
                m_triangles.Add(added);     // Left vert for triangle
                m_triangles.Add(added + 1); // Right vert for triangle
                added++;
            }
        }
        m_triangles[m_triangles.Count - 1] = 1; // Correct the last triangle

        // Sorts from index 1 to the end of the array (skips the player position
        // Player position is always indexed as 0;
        m_hit_points = m_hit_points.OrderBy(p => Math.Atan2(p.y - position.y, p.x - position.x)).ToList();
        m_hit_points.Insert(0, position);

        m_shadow_Renderer.Triangles = m_triangles.ToArray();
        m_shadow_Renderer.Vertices = m_hit_points.ToArray();
    }
}
