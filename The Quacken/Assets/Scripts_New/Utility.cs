using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


class Utility
{
    public static Mesh Create_Mesh(out GameObject p_mesh_game_object, out GameObject p_collider_game_object, 
                                   string p_mesh_name, string p_collider_name, 
                                   string p_mesh_layer, string p_collider_layer,
                                   Material p_material)
    {
        p_collider_game_object = new GameObject(p_collider_name);
        Rigidbody2D temp = p_collider_game_object.AddComponent<Rigidbody2D>();
        temp.bodyType = RigidbodyType2D.Static;

        Mesh mesh = Create_Mesh(out p_mesh_game_object, p_mesh_name, p_mesh_layer, p_material);
        p_collider_game_object.transform.parent = p_mesh_game_object.transform;
        p_collider_game_object.layer = LayerMask.NameToLayer(p_collider_layer);

        p_collider_game_object.AddComponent<PolygonCollider2D>().isTrigger = true;

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

    public class Sort_Points_By
    {
        public class Angle
        {
            static public int Partition(Shadow.Point[] arr, int low, int high, Vector3 origin)
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
                        Shadow.Point temp1 = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp1;
                    }
                }
                Shadow.Point temp2 = arr[i + 1];
                arr[i + 1] = arr[high];
                arr[high] = temp2;

                return i + 1;
            }

            public static void Quick_Sort(Shadow.Point[] arr, int low, int high, Vector3 origin)
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
}

