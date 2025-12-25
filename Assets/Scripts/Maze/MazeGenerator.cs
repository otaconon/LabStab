using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze {
    public abstract class MazeGenerator : MonoBehaviour {
        [SerializeField] protected int _width;
        [SerializeField] protected int _height;


        public Texture2D GenerateMazeTexture() {
            if (_width % 2 == 0) _width++;
            if (_height % 2 == 0) _height++;

            var texture = new Texture2D(_width, _height, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            int[,] maze = GenerateMaze();
            var pixels = new Color32[_height * _width];

            for (var y = 0; y < _height; y++) {
                for (var x = 0; x < _width; x++) {
                    var val = (byte)(maze[y, x] * 255);
                    pixels[y * _width + x] = new Color32(val, val, val, 255);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();

            return texture;
        }

        protected abstract int[,] GenerateMaze();

        protected List<Vector2Int> GetNeighbors(int[,] maze, Vector2Int position, Vector2Int[] directions) {
            var neighbors = new List<Vector2Int>();
            foreach (Vector2Int dir in directions) {
                Vector2Int neighbor = position + dir;
                if (neighbor.x > 0 && neighbor.x < _width - 1 && neighbor.y > 0 && neighbor.y < _height - 1 &&
                    maze[neighbor.y, neighbor.x] == 0) {
                    neighbors.Add(dir);
                }
            }

            return neighbors;
        }
    }
}