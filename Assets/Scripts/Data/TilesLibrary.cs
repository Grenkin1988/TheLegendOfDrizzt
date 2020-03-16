using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace TheLegendOfDrizzt.Data {
    public class TilesLibrary {
        private static TilesLibrary _instance;
        private static readonly object _padlock = new object();
        private const char NAME_SEPARATOR = '_';
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(TileData[]));

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

        public TileData[] GetDoubleTile(string name) {
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
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/SimpleTiles.xml");

            if (File.Exists(filePath)) {
                string xml = File.ReadAllText(filePath);
                var loadedData = (TileData[])_serializer.Deserialize(new StringReader(xml));

                foreach (var tileData in loadedData) {
                    _simpleTiles[tileData.Name] = tileData;
                }
            } else {
                Debug.LogError("Cannot Simple Tiles data!");
            }
        }

        private void LoadSpecialTiles() {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/SpecialTiles.xml");

            if (File.Exists(filePath)) {
                string xml = File.ReadAllText(filePath);
                var loadedData = (TileData[])_serializer.Deserialize(new StringReader(xml));

                foreach (var tileData in loadedData) {
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
            return new[] {
                tileData.Name
            };
        }

        private void LoadDoubleTiles() {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/DoubleTiles.xml");

            if (File.Exists(filePath)) {
                string xml = File.ReadAllText(filePath);
                var loadedData = (TileData[])_serializer.Deserialize(new StringReader(xml));

                foreach (var tileData in loadedData) {
                    string[] name = GetTileDataName(tileData);
                    if (!_doubleTiles.ContainsKey(name[0])) {
                        _doubleTiles[name[0]] = new List<TileData>();
                    }
                    if (name.Length > 1
                        && int.TryParse(name[1], out int result)
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
