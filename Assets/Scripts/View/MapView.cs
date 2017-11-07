using System;
using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.View {
    public class MapView : IDisposable {
        private Map _map;
        private GameObject _adventureMapGameObject;

        private readonly Dictionary<Tile, TileView> TileViewModels = new Dictionary<Tile, TileView>();

        public MapView(Map map) {
            _map = map;
            InitializeMapView();
        }

        private void InitializeMapView() {
            _map.NewTileCreated += MapOnNewTileCreated;
            _adventureMapGameObject = new GameObject("AdventureMap");
            _adventureMapGameObject.transform.SetParent(GameObject.Find("_Dynamic").transform);
            _adventureMapGameObject.transform.position = new Vector3();

            foreach (Tile tile in _map.GetAllTiles()) {
                var tileViewModel = new TileView(tile, _adventureMapGameObject.transform);
                tileViewModel.Draw();
                TileViewModels.Add(tile, tileViewModel);
            }
        }

        public Tile GetTileByGameObject(GameObject tileGameObject) {
            return TileViewModels.FirstOrDefault(pair => pair.Value.TileGameObject.Equals(tileGameObject)).Key;
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
