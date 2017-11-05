using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace ViewModel {
    public class MapViewModel : MonoBehaviour {
        private Map Map;
        private MouseController MouseController;

        private TileStack Deck;
        private readonly Dictionary<Tile, TileViewModel> TileViewModels = new Dictionary<Tile, TileViewModel>();
        public static readonly Dictionary<string, Sprite> SpritesMap = new Dictionary<string, Sprite>();

        private void Start() {
            Deck = new TileStack();
            Deck.SetSpecialTile("UndergroundRiver", 8);
            Deck.GenerateTileStack();
            Deck.ShuffleTileStack();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Tiles");
            foreach (Sprite sprite in sprites) {
                SpritesMap.Add(sprite.name, sprite);
            }

            MouseController = GameObject.FindGameObjectWithTag("MouseController").GetComponent<MouseController>();
            if (MouseController == null) { throw new NullReferenceException("No MouseController found"); }
            MouseController.TileClicked += MouseControllerOnTileClicked;

            Map = new Map();
            Map.NewTileCreated += MapOnNewTileCreated;

            foreach (Tile tile in Map.GetAllTiles()) {
                var tileViewModel = new TileViewModel(tile, transform);
                tileViewModel.DrawTile();
                TileViewModels.Add(tile, tileViewModel);
            }
        }

        private void MouseControllerOnTileClicked(GameObject tileGameObject, int x, int y) {
            Tile tile = TileViewModels.FirstOrDefault(pair => pair.Value.TileGameObject.Equals(tileGameObject)).Key;
            if (tile == null) { return; }
            Directions? placementDirection;
            if (Map.IsValidPositionForNewTilePlacement(tile, x, y, out placementDirection)) {
                if (!placementDirection.HasValue) { return; }
                Tile newTile = Deck.GetNexTile();
                if (newTile != null) {
                    Map.PlaceNewTileNearExistent(tile, newTile, placementDirection.Value);
                }
            }
        }

        private void MapOnNewTileCreated(Tile tile) {
            var tileViewModel = new TileViewModel(tile, transform);
            tileViewModel.DrawTile();
            TileViewModels.Add(tile, tileViewModel);
        }

        private void OnDestroy() {
            if (MouseController != null) {
                MouseController.TileClicked -= MouseControllerOnTileClicked;
            }
            if (Map != null) {
                Map.NewTileCreated -= MapOnNewTileCreated;
            }
        }
    }
}
