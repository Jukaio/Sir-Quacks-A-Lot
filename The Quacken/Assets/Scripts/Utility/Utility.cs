using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}

