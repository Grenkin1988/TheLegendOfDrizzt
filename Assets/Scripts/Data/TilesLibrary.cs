using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TheLegendOfDrizzt.Assets.Scripts.Utility;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    public class TilesLibrary {
        private static TilesLibrary _instance;
        private static readonly object _padlock = new object();

        public static readonly TileData StartTileLeft = new TileData {
            Name = "StartTileLeft",
            Layout = "Wall;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall"
        };
        public static readonly TileData StartTileRight = new TileData {
            Name = "StartTileRight",
            Layout = "Wall;Floor;Floor;Floor;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall"
        };

        private readonly Dictionary<string, TileData> _simpleTiles;
        private readonly Dictionary<string, TileData> _specialTiles;

        public ReadOnlyDictionary<string, TileData> SimpleTiles => new ReadOnlyDictionary<string, TileData>(_simpleTiles);
        public ReadOnlyDictionary<string, TileData> SpecialTiles => new ReadOnlyDictionary<string, TileData>(_specialTiles);

        public static TilesLibrary Instance {
            get {
                if (_instance == null) {
                    lock (_padlock) {
                        if (_instance == null) {
                            _instance = new TilesLibrary();
                        }
                    }
                }
                return _instance;
            }
        }

        private TilesLibrary() {
            _simpleTiles = new Dictionary<string, TileData>();
            _specialTiles = new Dictionary<string, TileData>();
            InitializeLibrary();
        }

        private void InitializeLibrary() {
            LoadSimpleTiles();
            LoadSpecialTiles();
        }

        private void LoadSimpleTiles() {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/SimpleTiles.json");

            if (File.Exists(filePath)) {
                string dataAsJson = File.ReadAllText(filePath);
                TileData[] loadedData = JsonHelper.FromJson<TileData>(dataAsJson);

                foreach (TileData tileData in loadedData) {
                    _simpleTiles[tileData.Name] = tileData;
                }
            } else {
                Debug.LogError("Cannot Simple Tiles data!");
            }
        }

        private void LoadSpecialTiles() {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/SpecialTiles.json");

            if (File.Exists(filePath)) {
                string dataAsJson = File.ReadAllText(filePath);
                TileData[] loadedData = JsonHelper.FromJson<TileData>(dataAsJson);

                foreach (TileData tileData in loadedData) {
                    _specialTiles[tileData.Name] = tileData;
                }
            } else {
                Debug.LogError("Cannot Special Tiles data!");
            }
        }
    }
}
