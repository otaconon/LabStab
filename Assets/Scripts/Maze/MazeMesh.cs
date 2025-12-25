using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class MazeMesh : MonoBehaviour {
        [SerializeField] private float _scale;
        [SerializeField] private float _height;
        
        private Texture2D _mazeTexture;
    
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private int _vertexCount;

        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;

        public void GenerateMesh(Texture2D mazeTexture) {
            _mazeTexture = mazeTexture;
            
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uvs = new List<Vector2>();
            _vertexCount = 0;

            for (var x = 0; x < mazeTexture.width; x++)
            {
                for (var z = 0; z < mazeTexture.height; z++) {
                    if (mazeTexture.GetPixel(x, z).grayscale > 0.5f) continue;
                
                    var pos = new Vector3(x * _scale, 0, z * _scale);
                    CreateBlock(x, z, pos);
                }
            }

            UpdateMesh();
        }

        private void UpdateMesh() {
            var mesh = new Mesh {
                vertices = _vertices.ToArray(),
                triangles = _triangles.ToArray(),
                uv = _uvs.ToArray()
            };

            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;
           GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private void CreateBlock(int x, int z, Vector3 pos) {
            AddFace(pos + Vector3.up * _height, Vector3.forward, Vector3.right, false, false);

            // Front Face
            if (IsGap(x, z + 1))
                AddFace(pos + Vector3.forward * _scale, Vector3.up, Vector3.right, true, true);

            // Back Face
            if (IsGap(x, z - 1))
                AddFace(pos, Vector3.up, Vector3.right, true, false);

            // Right Face
            if (IsGap(x + 1, z))
                AddFace(pos + Vector3.right * _scale, Vector3.up, Vector3.forward, true, false);

            // Left Face
            if (IsGap(x - 1, z))
                AddFace(pos, Vector3.up, Vector3.forward, true, true);
        }


        private bool IsGap(int x, int z) {
            if (x < 0 || x >= _mazeTexture.width || z < 0 || z >= _mazeTexture.height)
                return true;

            return _mazeTexture.GetPixel(x, z).grayscale > 0.5f;
        }

        // Generates face in clockwise order
        private void AddFace(Vector3 corner, Vector3 upDir, Vector3 rightDir, bool isSideWall, bool flip) {
            Vector3 u = rightDir * _scale;
            Vector3 v = isSideWall ? upDir * _height : upDir * _scale;

            _vertices.Add(corner);
            _vertices.Add(corner + v);
            _vertices.Add(corner + u + v);
            _vertices.Add(corner + u);

            if (flip) {
                _triangles.Add(_vertexCount + 0);
                _triangles.Add(_vertexCount + 2);
                _triangles.Add(_vertexCount + 1);

                _triangles.Add(_vertexCount + 0);
                _triangles.Add(_vertexCount + 3);
                _triangles.Add(_vertexCount + 2);
            }
            else {
                _triangles.Add(_vertexCount + 0);
                _triangles.Add(_vertexCount + 1);
                _triangles.Add(_vertexCount + 2);

                _triangles.Add(_vertexCount + 0);
                _triangles.Add(_vertexCount + 2);
                _triangles.Add(_vertexCount + 3);
            }

            _uvs.Add(new Vector2(0, 0));
            _uvs.Add(new Vector2(0, 1));
            _uvs.Add(new Vector2(1, 1));
            _uvs.Add(new Vector2(1, 0));

            _vertexCount += 4;
        }
    }
}