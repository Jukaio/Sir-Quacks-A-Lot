using Utility;
using System.Collections.Generic;
using UnityEngine;

class Visibility
{
    static bool Visible(Point p_point, List<Segment> p_edges, Vector2 p_position)
    {
        foreach (var edge in p_edges)
        {
            if (edge.m_id == p_point.first_segment || edge.m_id == p_point.second_segment)
                continue;

            if (Extra_Collision.Line_Line_Intersection(edge, new Segment(p_position, p_point.m_coordinate)))
            {
                return false;
            }
        }

        return true;
    }

    static public List<Point> Find_Visible_Points(List<List<Segment>> p_segments, List<Point> p_points, Vector2 p_position)
    {
        Segment halfline = new Segment(p_position, (2000.0f * Vector2.left) + (p_position * Vector2.up));
        List<Segment> edges = new List<Segment>();
        List<Point> visible_points = new List<Point>();

        foreach (List<Segment> list in p_segments)
        {
            foreach (Segment line in list)
            {
                if (Extra_Collision.Line_Line_Intersection(halfline, line))
                {
                    edges.Add(line);
                }
            }
        }
        for (int i = 0; i < p_points.Count; i++)
        {
            Point point = p_points[i];

            if (Visible(point, edges, p_position))
                visible_points.Add(point);

            double main_angle = Angle.Angle_Between_Segments(point.m_coordinate, halfline.m_end, p_position);

            if (point.first_segment > -1)
            {
                var first_segment = p_segments[point.m_polygon_id][point.first_segment];

                Vector2 first_segment_end = (point.m_coordinate != first_segment.m_end) ? first_segment.m_end : first_segment.m_start;
                double first_seg_angle = Angle.Angle_Between_Segments(first_segment_end, halfline.m_end, p_position);

                if (first_seg_angle > main_angle)
                    edges.Add(first_segment);
                else
                    edges.Remove(first_segment);
            }

            if (point.second_segment > -1)
            {
                var second_segment = p_segments[point.m_polygon_id][point.second_segment];

                Vector2 second_segment_end = (point.m_coordinate != second_segment.m_end) ? second_segment.m_end : second_segment.m_start;
                double second_seg_angle = Angle.Angle_Between_Segments(second_segment_end, halfline.m_end, p_position);

                if (second_seg_angle > main_angle)
                    edges.Add(second_segment);
                else
                    edges.Remove(second_segment);
            }
        }
        return visible_points;
    }
}

