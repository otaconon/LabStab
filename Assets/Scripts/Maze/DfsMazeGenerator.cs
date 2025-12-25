using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    public class DfsMazeGenerator : MazeGenerator
    {
        protected override int[,] GenerateMaze() {
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
