using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LabyrinthDebug : MonoBehaviour
{
    [SerializeField]
    private RawImage outputImage;

    [ContextMenu("Generate Test Texture")]
    public void GenerateNow()
    {
        Texture2D tex = MazeGenerator.GenerateMazeTexture(64, 64);
        
        tex.filterMode = FilterMode.Point;
        outputImage.texture = tex;
        
        Debug.Log("Texture generated successfully!");
    }
}
