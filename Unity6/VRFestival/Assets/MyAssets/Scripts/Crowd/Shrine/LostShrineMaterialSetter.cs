#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// LostShrineMaterialSetter Class
/// </summary>
public class LostShrineMaterialSetter : EditorWindow
{
    #region Methods

    #region Private Methods

    // Creates a custom menu item under the Assets menu in the editor
    [MenuItem("Assets/lost-shrine Materials Setter")]
    private static void Open()
    {
        // Find all Texture2D assets within the "Assets/MyAssets/3D_Models/Crowd/Shrine/textures" folder and its subfolders
        string[] texture_guids = AssetDatabase.FindAssets("t: Texture2D", new[] { "Assets/MyAssets/3D_Models/Crowd/Shrine/textures" });
        // Convert the GUIDs (unique identifiers) to actual Texture2D objects
        Texture2D[] textures = texture_guids.Select
            (
                guid =>
                {
                    // Get the asset path from the GUID
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    // Load the Texture2D asset from the path
                    return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                }
            ).ToArray();
        // Find all Material assets within the "Assets/MyAssets/3D_Models/Crowd/Shrine" folder and its subfolders
        string[] material_guids = AssetDatabase.FindAssets("t: Material", new[] { "Assets/MyAssets/3D_Models/Crowd/Shrine/Materilas" });
        foreach (var guid in material_guids)
        {
            // Get the asset path from the GUID
            var path = AssetDatabase.GUIDToAssetPath(guid);
            // Load the Material asset from the path
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var name = material.name.Replace(" ", "_");
            // Set specific textures for the material
            SetTexture(material, textures, name, "_albedo", "_BaseMap");
            SetTexture(material, textures, name, "_metallic", "_MetallicGlossMap");
            SetTexture(material, textures, name, "_normal", "_BumpMap");
            SetTexture(material, textures, name, "_AO", "_OcclusionMap");
        }
        // Save any changes made to the assets in the project
        AssetDatabase.SaveAssets();
    }

    // Assigns a specific texture to a material property based on naming conventions
    private static void SetTexture(Material material, Texture2D[] textures, string name, string postfix, string textureName)
    {
        // Combine the material name and postfix to create a search string
        var seed = name + postfix;
        // Filter the textures array to find a texture with a matching name
        var tex = textures.Where(texture => texture.name == seed);
        // If a matching texture is found, set it to the specified property of the material
        if (tex.Count() > 0) material.SetTexture(textureName, tex.First());
    }

    #endregion Private Methods

    #endregion Methods
}

#endif