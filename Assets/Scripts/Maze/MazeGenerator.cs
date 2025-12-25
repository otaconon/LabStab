using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze {
    public abstract class MazeGenerator : MonoBehaviour {
        [SerializeField] protected int width;
        [SerializeField] protected int height;


        public Texture2D GenerateMazeTexture() {
            if (width % 2 == 0) width++;
            if (height % 2 == 0) height++;

            var texture = new Texture2D(width, height, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            int[,] maze = GenerateMaze();
            var pixels = new Color32[height * width];

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var val = (byte)(maze[y, x] * 255);
                    pixels[y * width + x] = new Color32(val, val, val, 255);
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
                if (neighbor.x > 0 && neighbor.x < width - 1 && neighbor.y > 0 && neighbor.y < height - 1 &&
                    maze[neighbor.y, neighbor.x] == 0) {
                    neighbors.Add(dir);
                }
            }

            return neighbors;
        }
    }
}