using Utility;
using System.Collections.Generic;
using UnityEngine;
using CSKicksCollection.Trees;
using System;

class Visibility
{
    struct Key_Segment : IComparable
    {
        public Key_Segment(double p_key, Segment p_segment)
        {
            m_pair = new KeyValuePair<double, Segment>(p_key, p_segment);
        }

        private KeyValuePair<double, Segment> m_pair;
        public double Key
        {
            get { return m_pair.Key; }
        }
        public Segment Segment
        {
            get { return m_pair.Value; }
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(Key_Segment))
                return 0;
            Key_Segment temp = (Key_Segment)obj;
            return Key.CompareTo(temp.Key);
        }

        public override bool Equals(object obj)
        {
            return obj is Key_Segment segment &&
                   EqualityComparer<KeyValuePair<double, Segment>>.Default.Equals(m_pair, segment.m_pair);
        }
    }

    static bool Visible(Point p_point, SortedSet<Key_Segment> p_tree, Vector2 p_position)
    {
        Segment edge = p_tree.Min.Segment;
        if (Extra_Collision.Line_Line_Intersection_Excluding_Ends(edge, new Segment(p_position, p_point.m_coordinate)))
        {
            return false;
        }
        else
            return true;
    }

    public static List<Point> Find_Visible_Points(List<List<Segment>> p_polygons, List<Point> p_points, Vector2 p_position)
    {
        SortedSet<Key_Segment> set = new SortedSet<Key_Segment>();
        Segment halfline = new Segment(p_position, (2000.0f * Vector2.left) + (p_position * Vector2.up));

        List<Point> visible_points = new List<Point>();

        foreach (List<Segment> segments in p_polygons)
        {
            foreach (Segment line in segments)
            {
                if (Extra_Collision.Line_Line_Intersection(halfline, line))
                {
                    double key = Math.Pow(line.m_start.x - p_position.x, 2) + Math.Pow(line.m_start.y - p_position.y, 2);
                    double key2 = Math.Pow(line.m_end.x - p_position.x, 2) + Math.Pow(line.m_end.y - p_position.y, 2);
                    Key_Segment temp = new Key_Segment(key + key2, line);
                    set.Add(temp);
                }
            }
        }

        for (int i = 0; i < p_points.Count; i++)
        {
            Point point = p_points[i];

            if (Visible(point, set, p_position))
                visible_points.Add(point);

            double main_angle = Angle.Angle_Between_Segments(point.m_coordinate, halfline.m_end, p_position);
           

            if (point.first_segment > -1)
            {
                var first_segment = p_polygons[point.m_polygon_id][point.first_segment];
                Check_Segment(first_segment, point, p_position, ref set);
            }

            if (point.second_segment > -1)
            {
                var second_segment = p_polygons[point.m_polygon_id][point.second_segment];
                Check_Segment(second_segment, point, p_position, ref set);
            }

        }
        return visible_points;
    }


    static void Check_Segment(Segment p_segment, Point p_point, Vector2 p_position, ref SortedSet<Key_Segment> p_tree)
    {
        double key2 = Math.Pow(p_segment.m_start.x - p_position.x, 2) + Math.Pow(p_segment.m_start.y - p_position.y, 2);
        double key = Math.Pow(p_segment.m_end.x - p_position.x, 2) + Math.Pow(p_segment.m_end.y - p_position.y, 2);
        Key_Segment key_segment = new Key_Segment( key + key2, p_segment);

        Vector2 segment_other_end = (p_point.m_coordinate != p_segment.m_end) ? p_segment.m_end : p_segment.m_start;
        double seg_angle = Angle.Determinant(segment_other_end, p_point.m_coordinate, p_position);

        if (seg_angle <= 0)
        {
            p_tree.Add(key_segment);
        }
        else
        {

            p_tree.Remove(key_segment);
        }
    }
}

