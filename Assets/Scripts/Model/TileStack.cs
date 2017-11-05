using System.Collections.Generic;
using System.Linq;
using Model;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Utility;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class TileStack {
        private const int TIMES_TO_SHUFFLE = 5;
        private readonly Stack<TileData> _deck;
        private readonly TilesLibrary TilesLibrary;

        private List<TileData> _allTilesWithoutSpecial;
        private string _specialTileName;
        private int _specialTilePosition;

        public TileStack() {
            TilesLibrary = TilesLibrary.Instance;
            _deck = new Stack<TileData>();
            _allTilesWithoutSpecial = new List<TileData>();
        }

        public void SetSpecialTile(string tileName, int tilePositionAfterShuffle) {
            _specialTileName = tileName;
            _specialTilePosition = tilePositionAfterShuffle;
        }

        public void GenerateTileStack() {
            _allTilesWithoutSpecial.AddRange(TilesLibrary.SimpleTiles.Values);
            _allTilesWithoutSpecial.AddRange(TilesLibrary.SpecialTiles
                .Where(pair => pair.Key != _specialTileName)
                .Select(pair => pair.Value));
        }

        public void ShuffleTileStack() {
            if (_allTilesWithoutSpecial.Count == 0) {
                Debug.LogError($"Tile stack is empty! Run {nameof(GenerateTileStack)} first");
            }
            TileData[] tilesToShuffle = _allTilesWithoutSpecial.ToArray();
            for (int i = 0; i < TIMES_TO_SHUFFLE; i++) {
                FisherYatesShuffle.ShuffleSequence(tilesToShuffle);
            }
            _allTilesWithoutSpecial = new List<TileData>(tilesToShuffle);
            if (!string.IsNullOrEmpty(_specialTileName) && _specialTilePosition > 0) {
                AddSpecialTileAtPosition(_specialTileName, _specialTilePosition);
            }
            AddTilesToStack();
        }

        public Tile GetNexTile() {
            if (_deck.Count != 0) {
                TileData tile = _deck.Pop();
                Debug.Log($"Next tile is: {tile}");
                return new Tile(tile);
            } else {
                Debug.LogError("No more Tiles in stack");
                return null;
            }
        }

        private void AddSpecialTileAtPosition(string tileName, int tilePositionAfterShuffle) {
            if (!TilesLibrary.SpecialTiles.ContainsKey(tileName)) {
                Debug.LogError($"Specian tile with name: {tileName} do not exist in library!");
                return;
            }
            _allTilesWithoutSpecial.Insert(tilePositionAfterShuffle, TilesLibrary.SpecialTiles[tileName]);
        }

        private void AddTilesToStack() {
            for (int i = _allTilesWithoutSpecial.Count - 1; i >= 0; i--) {
                _deck.Push(_allTilesWithoutSpecial[i]);
            }
        }
    }
}
