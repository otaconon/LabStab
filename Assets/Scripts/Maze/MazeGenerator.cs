using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze {
    public class MazeGenerator : MonoBehaviour {
        private int _width;
        private int _height;

        public Texture2D GenerateMazeTexture(int width, int height, MazeGenerationType mazeType) {
            _width = width;
            _height = height;
            if (_width % 2 == 0) _width++;
            if (_height % 2 == 0) _height++;

            var texture = new Texture2D(_width, _height, TextureFormat.RGB24, false) {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            int[,] maze = mazeType switch {
                MazeGenerationType.BinaryTree => GenerateBinaryTreeMaze(),
                MazeGenerationType.Dfs => GenerateDfsMaze(),
                _ => throw new ArgumentOutOfRangeException(nameof(mazeType), mazeType, null)
            };

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

        private List<Vector2Int> GetNeighbors(int[,] maze, Vector2Int position, Vector2Int[] directions) {
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

        private int[,] GenerateBinaryTreeMaze() {
            var maze = new int[_height, _width];
            var directions = new Vector2Int[] { new(0, 2), new(2, 0)};
            for (var row = 1; row < _height-1; row+=2) {
                for (var col = 1; col < _width-1; col+=2) {
                    var position = new Vector2Int(row, col);
                    var neighbors = GetNeighbors(maze, new Vector2Int(row, col), directions);

                    maze[position.y, position.x] = 1;
                    if (neighbors.Count > 0) {
                        Vector2Int direction = neighbors[Random.Range(0, neighbors.Count)];
                        Vector2Int wall = position + (direction / 2);

                        maze[wall.y, wall.x] = 1;
                    }
                }
            }

            return maze;
        }

        private int[,] GenerateDfsMaze() {
            var maze = new int[_height, _width];
            var stack = new Stack<Vector2Int>();
            var start = new Vector2Int(1, 1);
            var directions = new Vector2Int[] { new(0, 2), new(2, 0), new(0, -2), new(-2, 0) };

            maze[start.y, start.x] = 1;
            stack.Push(start);
            while (stack.Count > 0) {
                Vector2Int current = stack.Peek();
                var neighbors = new List<Vector2Int>();

                foreach (Vector2Int dir in directions) {
                    Vector2Int neighbor = current + dir;

                    if (neighbor.x > 0 && neighbor.x < _width - 1 &&
                        neighbor.y > 0 && neighbor.y < _height - 1 &&
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
                }
                else {
                    stack.Pop();
                }
            }

            return maze;
        }
    }
}