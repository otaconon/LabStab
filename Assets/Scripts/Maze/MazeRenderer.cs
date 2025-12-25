using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    [RequireComponent(typeof(MazeMesh))]
    public class MazeRenderer : MonoBehaviour {
        [SerializeField] private MazeGenerator _mazeGenerator;
        
        private MazeMesh _mazeMesh;

        public void RenderMaze() {
            if (_mazeGenerator == null) {
                _mazeGenerator = GetComponent<MazeGenerator>();
            }

            if (_mazeMesh == null) {
                _mazeMesh = GetComponent<MazeMesh>();
            }

            Texture2D texture = _mazeGenerator.GenerateMazeTexture();
            _mazeMesh.GenerateMesh(texture);
        }
    }
}