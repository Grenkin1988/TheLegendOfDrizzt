using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using TheLegendOfDrizzt.Assets.Scripts.Utility;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    public class TilesLibrary {
        private static TilesLibrary _instance;
        private static readonly object _padlock = new object();
        private const char NAME_SEPARATOR = '_';

        private readonly Dictionary<string, TileData> _simpleTiles;
        private readonly Dictionary<string, TileData> _specialTiles;
        private readonly Dictionary<string, List<TileData>> _doubleTiles;

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
            _doubleTiles = new Dictionary<string, List<TileData>>();
            InitializeLibrary();
        }

        public TileData[] GetDubleTile(string name) {
            if (_doubleTiles.ContainsKey(name)) {
                return _doubleTiles[name].ToArray();
            }
            Debug.LogError($"DoubleTile with name {name} not found");
            return null;
        }

        private void InitializeLibrary() {
            LoadSimpleTiles();
            LoadSpecialTiles();
            LoadDoubleTiles();
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

        private string[] GetTileDataName(TileData tileData) {
            if (tileData.Name.Contains(NAME_SEPARATOR)) {
                return tileData.Name.Split(NAME_SEPARATOR);
            }
            return new []{ tileData.Name };
        }

        private void LoadDoubleTiles() {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/DoubleTiles.json");

            if (File.Exists(filePath)) {
                string dataAsJson = File.ReadAllText(filePath);
                TileData[] loadedData = JsonHelper.FromJson<TileData>(dataAsJson);
                foreach (TileData tileData in loadedData) {
                    string[] name = GetTileDataName(tileData);
                    if (!_doubleTiles.ContainsKey(name[0])) {
                        _doubleTiles[name[0]] = new List<TileData>();
                    }
                    int result;
                    if (name.Length > 1 
                        && int.TryParse(name[1], out result)
                        && result > 0) {
                        _doubleTiles[name[0]].Insert(result - 1, tileData);
                    } else {
                        _doubleTiles[name[0]].Add(tileData);
                    }
                }
            } else {
                Debug.LogError("Cannot Special Tiles data!");
            }
        }
    }
}
