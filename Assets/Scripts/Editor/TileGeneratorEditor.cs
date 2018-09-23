using JetBrains.Annotations;
using TheLegendOfDrizzt.Model;
using UnityEditor;
using UnityEngine;

namespace TheLegendOfDrizzt.Editor {
    public class TileGeneratorEditor : EditorWindow {
        private string _layout;

        [MenuItem("Window/Tile Generator")]
        [UsedImplicitly]
        private static void Init() {
            GetWindow(typeof(TileGeneratorEditor)).Show();
        }

        [UsedImplicitly]
        private void OnGUI() {
            _layout = EditorGUILayout.TextField("Layout", _layout, new GUIStyle {
                wordWrap = true,
                richText = true,
                stretchHeight = true
            }, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Clear")) {
                Clear();
            }
            if (GUILayout.Button("Wall")) {
                AddSquare(TerrainTypes.Wall);
            }
            if (GUILayout.Button("Floor")) {
                AddSquare(TerrainTypes.Floor);
            }
            if (GUILayout.Button("Mashrooms")) {
                AddSquare(TerrainTypes.Mashrooms);
            }
            if (GUILayout.Button("VolcanicVent")) {
                AddSquare(TerrainTypes.VolcanicVent);
            }
            if (GUILayout.Button("Chasm")) {
                AddSquare(TerrainTypes.Chasm);
            }
            if (GUILayout.Button("Pillar")) {
                AddSquare(TerrainTypes.Pillar);
            }
            if (GUILayout.Button("River")) {
                AddSquare(TerrainTypes.River);
            }
            if (GUILayout.Button("Bridge")) {
                AddSquare(TerrainTypes.Bridge);
            }
            if (GUILayout.Button("Crystal")) {
                AddSquare(TerrainTypes.Crystal);
            }
            if (GUILayout.Button("Campfire")) {
                AddSquare(TerrainTypes.Campfire);
            }
            if (GUILayout.Button("DwarfStatue")) {
                AddSquare(TerrainTypes.DwarfStatue);
            }

            if (GUILayout.Button("Copy")) {
                CopyToClipboard();
            }
        }

        private void Clear() {
            _layout = string.Empty;
        }

        private void AddSquare(TerrainTypes type) {
            if (string.IsNullOrEmpty(_layout)) {
                _layout += type;
            } else {
                _layout += $";{type}";
            }
        }

        private void CopyToClipboard() {
            var te = new TextEditor {
                text = _layout
            };
            te.SelectAll();
            te.Copy();
        }
    }
}
