using System;
using UnityEngine;

namespace Maze {
    [Serializable]
    public class MazeSettings {
        [Header("Texture Settings")]
        public MazeGenerationType mazeType;
        public int textureWidth = 16;
        public int textureHeight = 16;

        [Header("Mesh Settings")]
        public float scale = 1f;
        public float wallHeight = 2f;
    }
}
