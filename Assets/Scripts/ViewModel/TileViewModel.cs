﻿// =======================================================================
//  Copyright Glib "Grenkin" Kruglov 2016.
// =======================================================================

using Model;
using UnityEngine;

namespace ViewModel {
    public class TileViewModel {
        private readonly Tile Tile;
        private readonly Transform ParentTransform;

        public GameObject TileGameObject { get; private set; }

        public TileViewModel(Tile tile, Transform parentTransform) {
            Tile = tile;
            ParentTransform = parentTransform;
        }

        public void DrawTile() {
            TileGameObject = new GameObject("Tile_" + Tile.X + "_" + Tile.Y);
            TileGameObject.transform.position = new Vector3(Tile.X, Tile.Y, 0);
            TileGameObject.transform.SetParent(ParentTransform, true);

            for (var x = 0; x < 4; x++) {
                for (var y = 0; y < 4; y++) {
                    var squareGameObject = new GameObject("Square_" + x + "_" + y);
                    squareGameObject.transform.position = new Vector3(x, y, 0);
                    squareGameObject.transform.SetParent(TileGameObject.transform, false);
                    var boxCollier2D = squareGameObject.AddComponent<BoxCollider2D>();
                    boxCollier2D.offset = new Vector2(0.5f, 0.5f);
                    var squareRenderer = squareGameObject.AddComponent<SpriteRenderer>();
                    string tileTypeText = Tile[x, y].TerrainType.ToString();
                    if (MapViewModel.SpritesMap.ContainsKey(tileTypeText)) {
                        squareRenderer.sprite = MapViewModel.SpritesMap[tileTypeText];
                    }
                }
            }
        }
    }
}