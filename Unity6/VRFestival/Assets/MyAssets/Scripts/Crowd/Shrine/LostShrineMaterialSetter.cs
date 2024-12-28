#if UNITY_EDITOR

using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

/// <summary>
/// LostShrineMaterialSetter Class
/// </summary>
public class LostShrineMaterialSetter : EditorWindow
{
    #region Variables

    #region Private Variables

    private int selectBeforeShaderIndex;
    private int selectAfterShaderIndex;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Private Methods

    [MenuItem("Assets/lost-shrine Materials Setter")]
    private static void Open()
    {
        string[] texture_guids = AssetDatabase.FindAssets("t: Texture2D", new[] { "Assets/MyAssets/3D_Models/Crowd/Shrine/textures" });
        Texture2D[] textures = texture_guids.Select
            (
                guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                }
            ).ToArray();
        string[] guids = AssetDatabase.FindAssets("t: Material", new[] { "Assets/MyAssets/3D_Models/Crowd/Shrine" });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var name = material.name.Replace(" ", "_");
            SetTexture(material, textures, name, "_albedo", "_BaseMap");
            SetTexture(material, textures, name, "_metallic", "_MetallicGlossMap");
            SetTexture(material, textures, name, "_normal", "_BumpMap");
            SetTexture(material, textures, name, "_AO", "_OcclusionMap");
        }
        AssetDatabase.SaveAssets();
    }

    private static void SetTexture(Material material, Texture2D[] textures, string name, string postfix, string textureName)
    {
        var seed = name + postfix;
        var tex = textures.Where(texture => texture.name == seed);
        if (tex.Count() > 0) material.SetTexture(textureName, tex.First());
    }

    #endregion Private Methods

    #endregion Methods
}

#endif