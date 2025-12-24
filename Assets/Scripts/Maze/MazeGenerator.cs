using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze {
    public class MazeGenerator : MonoBehaviour {
        [NonSerialized] public Texture2D texture;

        public Texture2D GenerateMazeTexture(int width, int height, MazeGenerationType mazeType) {
            if (width % 2 == 0) width++;
            if (height % 2 == 0) height++;

            texture = new Texture2D(width, height, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            int[,] maze = mazeType switch {
                MazeGenerationType.Dfs => GenerateDfsMaze(width, height),
                _ => throw new ArgumentOutOfRangeException(nameof(mazeType), mazeType, null)
            };

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
    
        public static int[,] GenerateDfsMaze(int width, int height) {
            var maze = new int[height, width];
            var stack = new Stack<Vector2Int>();
            var start = new Vector2Int(1, 1);
            var directions = new Vector2Int[] {new(0, 2), new(2, 0), new(0, -2), new(-2, 0)};
        
            maze[start.y, start.x] = 1;
            stack.Push(start);
            while (stack.Count > 0) {
                Vector2Int current = stack.Peek();
                var neighbors = new List<Vector2Int>();

                foreach (Vector2Int dir in directions) {
                    Vector2Int neighbor = current + dir;
                
                    if (neighbor.x > 0 && neighbor.x < width - 1 && 
                        neighbor.y > 0 && neighbor.y < height - 1 && 
                        maze[neighbor.y, neighbor.x] == 0) {
                        neighbors.Add(dir);
                    }
                }

                if (neighbors.Count > 0) {
                    Vector2Int direction = neighbors[Random.Range(0, neighbors.Count)];
                    Vector2Int nextCell = current + direction;
                    Vector2Int wall = current + (direction / 2);
                
                    maze[nextCell.y, nextCell.x] = 1;
                    maze[wall.y, wall.x] = 1;
                    stack.Push(nextCell);
                } else {
                    stack.Pop();
                }
            }
        
            return maze;
        }
    }
}