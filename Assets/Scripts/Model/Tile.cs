using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLegendOfDrizzt.Assets.Scripts.Data;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Tile {
        public static readonly int TileSize = 4;
        private readonly Adjacent<Tile> AdjacentTiles;
        private Square[,] Squares;

        public Directions ArrowDirection { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Name { get; }

        public Square this[int x, int y] {
            get {
                if (x >= TileSize || x < 0) { throw new ArgumentOutOfRangeException("x"); }
                if (y >= TileSize || y < 0) { throw new ArgumentOutOfRangeException("y"); }
                return Squares[x, y];
            }
        }

        internal Tile(TileData tileData) {
            AdjacentTiles = new Adjacent<Tile>();
            Squares = new Square[TileSize, TileSize];
            Name = tileData.Name;
            FillTileByXML(tileData.Layout);
        }

        public void PlaceTile(int x, int y) {
            X = x;
            Y = y;
        }

        private void FillTileByXML(string squaresSequence) {
            string[] splittedSequence = squaresSequence.Split(';');
            if (splittedSequence.Length != TileSize * TileSize) { throw new InvalidOperationException("Squares sequence must contain 16 squares"); }
            int splittedSequenceIndex = 0;
            for (int x = 0; x < TileSize; x++) {
                for (int y = 0; y < TileSize; y++) {
                    string terrainTypeText = splittedSequence[splittedSequenceIndex];
                    var terrain = Enums.ParceEnumValue<TerrainTypes>(terrainTypeText);
                    Squares[x, y] = new Square(terrain);
                    splittedSequenceIndex++;
                }
            }
            ArrowDirection = Directions.South;
        }

        public void RotateTileClockwise() {
            var newArray = new Square[TileSize, TileSize];
            for (int i = TileSize - 1; i >= 0; --i) {
                for (int j = 0; j < TileSize; ++j) {
                    newArray[j, TileSize - 1 - i] = Squares[i, j];
                }
            }
            int newArrowDirection = (int)ArrowDirection + 1;
            if (newArrowDirection > 4) { newArrowDirection -= 4; }
            ArrowDirection = (Directions)newArrowDirection;
            Squares = newArray;
        }

        public void SetNeighbor(Tile tile, Directions direction) {
            AdjacentTiles.SetAdjacent(direction, tile);
        }

        public Tile GetNeighbor(Directions direction) {
            IList<Tile> adjacent = AdjacentTiles.GetAdjacent(direction);
            return adjacent != null && adjacent.Any() ? adjacent.First() : null;
        }
    }
}
