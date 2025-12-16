using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(MazeGenerator))]
    public class MazeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var generator = (MazeGenerator)target;

            GUILayout.Space(10);
            
            if (GUILayout.Button("Generate Maze Mesh"))
            {
                generator.GenerateMaze();
            }
        }
    }
}
