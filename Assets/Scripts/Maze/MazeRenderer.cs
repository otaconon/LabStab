using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    [RequireComponent(typeof(MazeGenerator), typeof(MazeMesh))]
    public class MazeRenderer : MonoBehaviour {
        [SerializeField] private MazeSettings settings;

        private MazeGenerator _mazeGenerator;
        private MazeMesh _mazeMesh;

        public void RenderMaze() {
            if (_mazeGenerator == null) {
                _mazeGenerator = GetComponent<MazeGenerator>();
            }

            if (_mazeMesh == null) {
                _mazeMesh = GetComponent<MazeMesh>();
            }

            Texture2D texture = _mazeGenerator.GenerateMazeTexture(settings.textureWidth, settings.textureHeight,
                settings.mazeType);
            _mazeMesh.GenerateMesh(texture, settings.scale, settings.wallHeight);
        }
    }
}