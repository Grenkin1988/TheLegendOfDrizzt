using System;
using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.View {
    public class MapView : IDisposable {
        private readonly Map _map;
        private GameObject _adventureMapGameObject;
        private Dictionary<Square, SquareView> _coloredSquareViews = new Dictionary<Square, SquareView>();

        private readonly Dictionary<Tile, TileView> _tileViewModels = new Dictionary<Tile, TileView>();

        public MapView(Map map) {
            _map = map;
            InitializeMapView();
        }

        public Tile GetTileByGameObject(GameObject tileGameObject) {
            return _tileViewModels.FirstOrDefault(pair => pair.Value.TileGameObject.Equals(tileGameObject)).Key;
        }

        public void DrawReachableZone(Character character) {
            foreach (var reachableSquare in character.BreadthFirstSearch.ReachableSquares) {
                var parent = reachableSquare.ParentTile;
                if (parent == null || !_tileViewModels.TryGetValue(parent, out var parentTileView)) { continue; }

                var squareView = parentTileView.GetSquareViewForSquare(reachableSquare);
                if (squareView == null) { continue; }
                squareView.SquareRenderer.color = Color.green;
                _coloredSquareViews[reachableSquare] = squareView;
            }
        }

        public void ResetReachableZone() {
            foreach (var coloredSquareView in _coloredSquareViews) {
                coloredSquareView.Value.SquareRenderer.color = Color.white;
                coloredSquareView.Key.DistanceFromStart = null;
            }
            _coloredSquareViews.Clear();
        }

        private void InitializeMapView() {
            _map.NewTileCreated += MapOnNewTileCreated;
            _adventureMapGameObject = new GameObject("AdventureMap");
            _adventureMapGameObject.transform.SetParent(GameObject.Find("_Dynamic").transform);
            _adventureMapGameObject.transform.position = new Vector3();

            foreach (var tile in _map.GetAllTiles().Values) {
                var tileViewModel = new TileView(tile, _adventureMapGameObject.transform);
                tileViewModel.Draw();
                _tileViewModels.Add(tile, tileViewModel);
            }
        }

        private void MapOnNewTileCreated(Tile tile) {
            var tileViewModel = new TileView(tile, _adventureMapGameObject.transform);
            tileViewModel.Draw();
            _tileViewModels.Add(tile, tileViewModel);
        }

        public void Dispose() {
            if (_map != null) {
                _map.NewTileCreated -= MapOnNewTileCreated;
            }
        }
    }
}
