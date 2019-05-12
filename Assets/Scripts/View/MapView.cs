using System;
using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.View {
    public class MapView : IDisposable {
        private Map _map;
        private GameObject _adventureMapGameObject;
        private Dictionary<Square, SquareView> ColoredSquareViews = new Dictionary<Square, SquareView>();

        private readonly Dictionary<Tile, TileView> TileViewModels = new Dictionary<Tile, TileView>();

        public MapView(Map map) {
            _map = map;
            InitializeMapView();
        }

        public Tile GetTileByGameObject(GameObject tileGameObject) {
            return TileViewModels.FirstOrDefault(pair => pair.Value.TileGameObject.Equals(tileGameObject)).Key;
        }

        public void DrawReachableZone(Character character) {
            foreach (Square reachableSquare in character.BreadthFirstSearch.ReachableSquares) {
                Tile parent = reachableSquare.ParentTile;
                TileView parentTileView;
                if (parent == null || !TileViewModels.TryGetValue(parent, out parentTileView)) { continue; }

                SquareView squareView = parentTileView.GetSquareViewForSquare(reachableSquare);
                if (squareView == null) { continue; }
                squareView.SquareRenderer.color = Color.green;
                ColoredSquareViews[reachableSquare] = squareView;
            }
        }

        public void ResetReachableZone() {
            foreach (KeyValuePair<Square, SquareView> coloredSquareView in ColoredSquareViews) {
                coloredSquareView.Value.SquareRenderer.color = Color.white;
                coloredSquareView.Key.DistanceFromStart = null;
            }
            ColoredSquareViews.Clear();
        }

        private void InitializeMapView() {
            _map.NewTileCreated += MapOnNewTileCreated;
            _adventureMapGameObject = new GameObject("AdventureMap");
            _adventureMapGameObject.transform.SetParent(GameObject.Find("_Dynamic").transform);
            _adventureMapGameObject.transform.position = new Vector3();

            foreach (Tile tile in _map.GetAllTiles().Values) {
                var tileViewModel = new TileView(tile, _adventureMapGameObject.transform);
                tileViewModel.Draw();
                TileViewModels.Add(tile, tileViewModel);
            }
        }

        private void MapOnNewTileCreated(Tile tile) {
            var tileViewModel = new TileView(tile, _adventureMapGameObject.transform);
            tileViewModel.Draw();
            TileViewModels.Add(tile, tileViewModel);
        }

        public void Dispose() {
            if (_map != null) {
                _map.NewTileCreated -= MapOnNewTileCreated;
            }
        }
    }
}
