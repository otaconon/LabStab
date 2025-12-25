using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    [RequireComponent(typeof(MazeMesh))]
    public class MazeRenderer : MonoBehaviour {
        [SerializeField] private MazeGenerator mazeGenerator;
        
        private MazeMesh _mazeMesh;

        public void RenderMaze() {
            if (mazeGenerator == null) {
                mazeGenerator = GetComponent<MazeGenerator>();
            }

            if (_mazeMesh == null) {
                _mazeMesh = GetComponent<MazeMesh>();
            }

            Texture2D texture = mazeGenerator.GenerateMazeTexture();
            _mazeMesh.GenerateMesh(texture);
        }
    }
}