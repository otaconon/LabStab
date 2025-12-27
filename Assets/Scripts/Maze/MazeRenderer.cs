using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Maze {
    [RequireComponent(typeof(MazeMesh))]
    public class MazeRenderer : MonoBehaviour {
        [SerializeField] private MazeGenerator _mazeGenerator;
        [SerializeField] private string _textureSavePath;

        private MazeMesh _mazeMesh;

        public void RenderMaze() {
            if (_mazeGenerator == null) {
                _mazeGenerator = GetComponent<MazeGenerator>();
            }

            if (_mazeMesh == null) {
                _mazeMesh = GetComponent<MazeMesh>();
            }

            Texture2D ramTexture = _mazeGenerator.GenerateMazeTexture();
            SaveTextureAsAsset(ramTexture);
            
            _mazeMesh.GenerateMesh(ramTexture);
        }

        private Texture2D SaveTextureAsAsset(Texture2D texture) {
#if UNITY_EDITOR
            byte[] bytes = texture.EncodeToPNG();

            File.WriteAllBytes(_textureSavePath, bytes);

            AssetDatabase.Refresh();

            var savedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(_textureSavePath);

            var importer = (TextureImporter)AssetImporter.GetAtPath(_textureSavePath);
            if (importer != null) {
                importer.isReadable = true;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }

            return savedTexture;
#else
#endif
        }
    }
}