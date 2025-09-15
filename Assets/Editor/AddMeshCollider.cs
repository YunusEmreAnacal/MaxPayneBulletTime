using UnityEngine;
using UnityEditor;

public class AddMeshCollider
{
    [MenuItem("Tools/Add Mesh Colliders to Children")]
    static void AddColliders()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Lütfen bir GameObject seçin!");
            return;
        }

        GameObject parent = Selection.activeGameObject;
        MeshRenderer[] meshes = parent.GetComponentsInChildren<MeshRenderer>();

        int count = 0;
        foreach (MeshRenderer mr in meshes)
        {
            GameObject go = mr.gameObject;
            if (go.GetComponent<MeshFilter>() && go.GetComponent<MeshCollider>() == null)
            {
                MeshCollider mc = go.AddComponent<MeshCollider>();
                mc.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
                count++;
            }
        }

        Debug.Log($"✅ {count} adet Mesh Collider eklendi: {parent.name}");
    }
}
