using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MazeGenerator : MonoBehaviour {
    
    [Header("Settings")]
    public float scale = 1f;
    public float wallHeight = 2f;
    public int resX = 16;
    public int resY = 16;
    
    private Texture2D _mazeTexture;
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Vector2> _uvs;
    private int _vertexCount;
    
    public void GenerateMaze() {
        _mazeTexture = GenerateMazeTexture(resX, resY);

        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _uvs = new List<Vector2>();
        _vertexCount = 0;

        for (var x = 0; x < _mazeTexture.width; x++)
        {
            for (var z = 0; z < _mazeTexture.height; z++) {
                if (!(_mazeTexture.GetPixel(x, z).grayscale > 0.5f)) continue;
                
                var pos = new Vector3(x * scale, 0, z * scale);
                CreateBlock(x, z, pos);
            }
        }

        UpdateMesh();
    }

    private void CreateBlock(int x, int z, Vector3 pos)
    {
        AddFace(pos + Vector3.up * wallHeight, Vector3.forward, Vector3.right, false, false);

        // Front Face
        if (IsGap(x, z + 1)) 
            AddFace(pos + Vector3.forward * scale, Vector3.up, Vector3.right, true, true);

        // Back Face
        if (IsGap(x, z - 1)) 
            AddFace(pos, Vector3.up, Vector3.right, true, false);

        // Right Face
        if (IsGap(x + 1, z)) 
            AddFace(pos + Vector3.right * scale, Vector3.up, Vector3.forward, true, false);

        // Left Face
        if (IsGap(x - 1, z)) 
            AddFace(pos, Vector3.up, Vector3.forward, true, true);
    }

    private bool IsGap(int x, int z)
    {
        if (x < 0 || x >= _mazeTexture.width || z < 0 || z >= _mazeTexture.height) 
            return true;

        return _mazeTexture.GetPixel(x, z).grayscale < 0.5f;
    }
    
    // Generates face in clockwise order
    private void AddFace(Vector3 corner, Vector3 upDir, Vector3 rightDir, bool isSideWall, bool flip)
    {
        Vector3 u = rightDir * scale;
        Vector3 v = isSideWall ? upDir * wallHeight : upDir * scale;

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

    private void UpdateMesh()
    {
        var mesh = new Mesh {
            vertices = _vertices.ToArray(),
            triangles = _triangles.ToArray(),
            uv = _uvs.ToArray()
        };

        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    public static Texture2D GenerateMazeTexture(int resX, int resY) {
        if (resX % 2 == 0) resX++;
        if (resY % 2 == 0) resY++;

        var tex = new Texture2D(resX, resY, TextureFormat.RGB24, false) {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        var maze = GenerateDfsMaze(resX, resY);
        var pixels = new Color32[resY * resX];

        for (var y = 0; y < resY; y++) {
            for (var x = 0; x < resX; x++) {
                byte val = (byte)(maze[y, x] * 255);
                pixels[y * resX + x] = new Color32(val, val, val, 255);
            }
        }
        
        tex.SetPixels32(pixels);
        tex.Apply();

        return tex;
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