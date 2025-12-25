using UnityEngine;

namespace Maze {
    public class BinaryMazeGenerator : MazeGenerator {
        protected override int[,] GenerateMaze() {
            var maze = new int[_height, _width];
            var directions = new Vector2Int[] { new(0, 2), new(2, 0) };
            for (var row = 1; row < _height - 1; row += 2) {
                for (var col = 1; col < _width - 1; col += 2) {
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
    }
}