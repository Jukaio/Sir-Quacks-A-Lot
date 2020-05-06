using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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

    public static bool operator ==(Point lhs, Point rhs)
    {
        return lhs.m_coordinate == rhs.m_coordinate;
    }

    public static bool operator !=(Point lhs, Point rhs)
    {
        return lhs.m_coordinate != rhs.m_coordinate;
    }

    public override bool Equals(object obj)
    {
        return obj is Point point &&
               m_coordinate.Equals(point.m_coordinate);
    }

    public override int GetHashCode()
    {
        return -1762665565 + m_coordinate.GetHashCode();
    }
}

public struct Segment : IComparable // Could add a function to swap start and end according to Atan2
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



namespace Utility
{
    public class Extra_Mesh
    {
        public static Mesh Create_Mesh(out GameObject p_mesh_game_object, out PolygonCollider2D p_collider,
                                       string p_mesh_name, string p_collider_name,
                                       string p_mesh_layer, string p_collider_layer,
                                       Material p_material)
        {
            GameObject temp_collider_game_object = new GameObject(p_collider_name);
            Rigidbody2D temp = temp_collider_game_object.AddComponent<Rigidbody2D>();
            temp.bodyType = RigidbodyType2D.Static;

            Mesh mesh = Create_Mesh(out p_mesh_game_object, p_mesh_name, p_mesh_layer, p_material);
            temp_collider_game_object.transform.parent = p_mesh_game_object.transform;
            temp_collider_game_object.layer = LayerMask.NameToLayer(p_collider_layer);

            p_collider = temp_collider_game_object.AddComponent<PolygonCollider2D>();
            p_collider.isTrigger = true;

            return mesh;
        }

        public static Mesh Create_Mesh(out GameObject p_mesh_object, string p_name, string p_mesh_layer, Material p_material)
        {
            p_mesh_object = new GameObject(p_name);
            p_mesh_object.transform.position = Vector3.back;
            p_mesh_object.layer = LayerMask.NameToLayer(p_mesh_layer);

            Mesh mesh = new Mesh();
            MeshRenderer mesh_renderer = p_mesh_object.AddComponent<MeshRenderer>();
            MeshFilter m_mesh_filter = p_mesh_object.AddComponent<MeshFilter>();

            m_mesh_filter.sharedMesh = mesh;

            Material material = new Material(p_material);
            mesh_renderer.material = material;
            return mesh;
        }

        public static void Update_Mesh(ref Mesh p_mesh, Vector3[] p_verts, int[] p_triangles)
        {
            p_mesh.Clear();
            p_mesh.vertices = p_verts;
            p_mesh.triangles = p_triangles;
        }
    }

    public class Sort_Points_By
    {
        public class Angle
        {
            static public int Partition(Point[] arr, int low, int high, Vector3 origin)
            {
                double angle;
                double pivot = Math.Atan2(arr[high].m_coordinate.y - origin.y, arr[high].m_coordinate.x - origin.x);

                int i = (low - 1);

                for (int j = low; j < high; j++)
                {
                    angle = Math.Atan2(arr[j].m_coordinate.y - origin.y, arr[j].m_coordinate.x - origin.x);
                    if (angle < pivot)
                    {
                        i++;
                        Point temp1 = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp1;
                    }
                }
                Point temp2 = arr[i + 1];
                arr[i + 1] = arr[high];
                arr[high] = temp2;

                return i + 1;
            }

            public static void Quick_Sort(Point[] arr, int low, int high, Vector3 origin)
            {
                if (low < high)
                {
                    int pivot = Partition(arr, low, high, origin);
                    Quick_Sort(arr, low, pivot - 1, origin);
                    Quick_Sort(arr, pivot + 1, high, origin);
                }
            }
        }
    }

    public class Sort_Vectors_By
    {
        public class Angle
        {
            static public int Partition(Vector3[] arr, int low, int high, Vector3 origin)
            {
                double angle;
                double pivot = Math.Atan2(arr[high].y - origin.y, arr[high].x - origin.x);

                int i = (low - 1);

                for (int j = low; j < high; j++)
                {
                    angle = Math.Atan2(arr[j].y - origin.y, arr[j].x - origin.x);
                    if (angle < pivot)
                    {
                        i++;
                        Vector3 temp1 = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp1;
                    }
                }
                Vector3 temp2 = arr[i + 1];
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

        public class Distance
        {
            static public int Partition(Vector3[] arr, int low, int high, Vector3 origin)
            {
                float distance;
                float pivot = origin.Distancef(arr[high]);

                int i = (low - 1);
                for (int j = low; j < high; j++)
                {
                    distance = origin.Distancef(arr[j]);
                    if (distance < pivot)
                    {
                        i++;
                        Vector3 temp1 = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp1;
                    }
                }
                Vector3 temp2 = arr[i + 1];
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
    }
    public class Bounds
    {
        static public Rect Create_Camera_Rect(UnityEngine.U2D.PixelPerfectCamera p_camera, Vector2 p_center, float p_offset)
        {
            float chw = (p_camera.refResolutionX / p_camera.assetsPPU) / 2.0f; // Camera half width
            float chh = (p_camera.refResolutionY / p_camera.assetsPPU) / 2.0f; // Camera half height

            return Rect.MinMaxRect(p_center.x - chw - p_offset, p_center.y - chh - p_offset, p_center.x + chw + p_offset, p_center.y + chh + p_offset);
        }
    }

    public class Extra_Collision
    {
        static public bool In_Bounds(Vector2 p_point, Rect p_bounds)
        {
            return ((p_point.x < p_bounds.xMax && p_point.x > p_bounds.xMin) &&
                    (p_point.y < p_bounds.yMax && p_point.y > p_bounds.yMin));
        }
        static public bool Polygon_Line_Collision(Segment p_segment, List<Segment> p_verts)
        {
            for (int current = 0; current < p_verts.Count; current++)
            {
                if (Line_Line_Intersection(p_segment, p_verts[current]))
                    return true;
            }
            return false;
        }
        static public bool Polygon_Line_Collision(List<Segment> p_verts, Segment p_segment)
        {
            return Polygon_Line_Collision(p_segment, p_verts);
        }
        static public bool Rectangle_Line_Collison(Segment p_segment, Rect p_rect)
        {
            float x_min = p_rect.xMin;
            float x_max = p_rect.xMax;
            float y_min = p_rect.yMin;
            float y_max = p_rect.yMax;

            Vector2 top_left = new Vector2(x_min, y_min);
            Vector2 top_right = new Vector2(x_max, y_min);
            Vector2 bottom_left = new Vector2(x_min, y_max);
            Vector2 bottom_right = new Vector2(x_max, y_max);

            if (Line_Line_Intersection(p_segment, new Segment(bottom_left, top_left)))
                return true;
            if (Line_Line_Intersection(p_segment, new Segment(top_right, bottom_right)))
                return true;
            if (Line_Line_Intersection(p_segment, new Segment(top_left, top_right)))
                return true;
            if (Line_Line_Intersection(p_segment, new Segment(bottom_right, bottom_left)))
                return true;

            return false;
        }
        static public bool Rectangle_Line_Collison(Rect p_rect, Segment p_segment)
        {
            return Rectangle_Line_Collison(p_segment, p_rect);
        }
        static public bool Line_Line_Intersection(Segment lhs, Segment rhs)
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
        static public bool Line_Line_Intersection(Segment lhs, Segment rhs, ref Vector2 p_intersection)
        {
            Vector2 lhs_start = lhs.m_start;
            Vector2 lhs_end = lhs.m_end;
            Vector2 rhs_start = rhs.m_start;
            Vector2 rhs_end = rhs.m_end;

            float uA = (((rhs_end.x - rhs_start.x) * (lhs_start.y - rhs_start.y) - (rhs_end.y - rhs_start.y) * (lhs_start.x - rhs_start.x)) /
                        ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));
            float uB = (((lhs_end.x - lhs_start.x) * (lhs_start.y - rhs_start.y) - (lhs_end.y - lhs_start.y) * (lhs_start.x - rhs_start.x)) /
                        ((rhs_end.y - rhs_start.y) * (lhs_end.x - lhs_start.x) - (rhs_end.x - rhs_start.x) * (lhs_end.y - lhs_start.y)));
            if (((uA >= 0 && uA <= 1) && (uB >= 0 && uB <= 1)))
            {
                p_intersection = lhs_start + (uA * (lhs_end - lhs_start));
                return true;
            }
            return false;
        }
    }

    public class Angle
    {
        static public double Angle_Between_Segments(Segment p_segment_lhs, Segment p_segment_rhs)
        {
            double angle1 = Math.Atan2(p_segment_lhs.m_start.y - p_segment_lhs.m_end.y,
                                       p_segment_lhs.m_start.x - p_segment_lhs.m_end.x);

            double angle2 = Math.Atan2(p_segment_rhs.m_start.y - p_segment_rhs.m_end.y,
                                       p_segment_rhs.m_start.x - p_segment_rhs.m_end.x);

            double temp = angle1 - angle2;
            double angle = temp < 0 ? Math.Abs(temp) + (Math.PI + temp) * 2 : temp;

            return angle;
        }
        static public double Angle_Between_Segments(Vector2 p_lhs, Vector2 p_rhs, Vector2 p_origin)
        {
            double angle1 = Math.Atan2(p_origin.y - p_lhs.y,
                                       p_origin.x - p_lhs.x);

            double angle2 = Math.Atan2(p_origin.y - p_rhs.y,
                                       p_origin.x - p_rhs.x);

            double temp = angle1 - angle2;
            double angle = temp < 0 ? Math.Abs(temp) + (Math.PI + temp) * 2 : temp;

            return angle;
        }
    }
}

