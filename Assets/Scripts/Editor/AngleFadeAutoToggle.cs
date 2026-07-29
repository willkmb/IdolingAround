using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public class AngleFadeAutoToggle
{
    static string[] materials =
    {
        "Assets/Mats/Tiling_Textures/New Material.mat",
        "Assets/Mats/Tiling_Textures/New Material 1.mat",
        "Assets/Mats/Tiling_Textures/New Material 2.mat",
        "Assets/Mats/Tiling_Textures/New Material 3.mat"
    };

    static AngleFadeAutoToggle() { EditorApplication.delayCall += EnableAngleFade; }


    [MenuItem("Tools/Enable angle fade")]
    static void EnableAngleFade()
    {
        foreach (string matPath in materials)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            string shaderPath = AssetDatabase.GetAssetPath(mat.shader);
            string shaderText = File.ReadAllText(shaderPath);

            if (shaderText.Contains("\"angleFade\": false"))
            {
                shaderText = shaderText.Replace("\"angleFade\": false","\"angleFade\": true");
                File.WriteAllText(shaderPath, shaderText);
            }
        }

        AssetDatabase.Refresh();
    }
}