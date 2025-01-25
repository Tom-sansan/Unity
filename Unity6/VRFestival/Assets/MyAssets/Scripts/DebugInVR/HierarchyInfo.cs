#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// HierarchyInfo Class
/// </summary>
public class HierarchyInfo : EditorWindow
{
    [MenuItem("Tools/Hierarchy Info")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyInfo>("Hierarchy Info");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Get Hierarchy Info"))
        {
            Transform[] allTransforms = FindObjectsOfType<Transform>();

            foreach (Transform transform in allTransforms)
            {
                string path = GetTransformPath(transform);
                Debug.Log(path);
            }
        }
    }

    private string GetTransformPath(Transform transform)
    {
        string path = transform.name;
        Transform parent = transform.parent;

        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }
}

#endif