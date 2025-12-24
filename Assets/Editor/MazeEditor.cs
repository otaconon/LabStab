using Maze;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(MazeRenderer))]
    public class MazeEditor : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var mazeRenderer = (MazeRenderer)target;
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Generate Maze Mesh"))
            {
                mazeRenderer.RenderMaze();
            }
        }
    }
}
