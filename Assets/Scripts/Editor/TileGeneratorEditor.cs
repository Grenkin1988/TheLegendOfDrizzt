﻿using UnityEngine;
using UnityEditor;
using Model;

public class TileGeneratorEditor : EditorWindow {
    private string layout;

    [MenuItem("Window/Tile Generator")]
    private static void Init() {
        GetWindow(typeof(TileGeneratorEditor)).Show();
    }

    private void OnGUI() {
        layout = EditorGUILayout.TextField("Layout", layout, new GUIStyle {
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

        if (GUILayout.Button("Copy")) {
            CopyToClipboard();
        }
    }

    private void Clear() {
        layout = string.Empty;
    }

    private void AddSquare(TerrainTypes type) {
        if (string.IsNullOrEmpty(layout)) {
            layout += type;
        } else {
            layout += $";{type}";
        }
    }

    private void CopyToClipboard() {
        var te = new TextEditor();
        te.text = layout;
        te.SelectAll();
        te.Copy();
    }
}
