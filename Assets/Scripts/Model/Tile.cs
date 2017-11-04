using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {
    public class Tile {
        public static readonly int TileSize = 4;
        private readonly Adjacent<Tile> AdjacentTiles;
        private Square[,] Squares;

        public Directions ArrowDirection { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Square this[int x, int y] {
            get {
                if (x >= TileSize || x < 0) { throw new ArgumentOutOfRangeException("x"); }
                if (y >= TileSize || y < 0) { throw new ArgumentOutOfRangeException("y"); }
                return Squares[x, y];
            }
        }

        internal Tile(string squaresSequence) {
            AdjacentTiles = new Adjacent<Tile>();
            Squares = new Square[TileSize, TileSize];
            FillTileByXML(squaresSequence);
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

        [Obsolete]
        public string DisplayTile() {
            int longestNameLength = Enums.GetValues<TerrainTypes>().Select(value => value.ToString().Length).Concat(new[] { 0 }).Max();
            var sb = new StringBuilder();
            for (int y = 3; y >= 0; y--) {
                for (int x = 0; x < 4; x++) {
                    string squareTerrainText = Squares[x, y].TerrainType.ToString();
                    int numberOfAdditionalSpace = longestNameLength - squareTerrainText.Length;
                    squareTerrainText += new string(' ', numberOfAdditionalSpace);
                    sb.Append(squareTerrainText);
                    if (x == 3) { continue; }
                    sb.Append("|||");
                }
                sb.Append("\n");
                if (y == 0) { continue; }
                sb.AppendLine(new string('-', longestNameLength * 4 + 3 * 3));
            }
            sb.AppendLine(new string('-', longestNameLength * 4 + 3 * 3));
            sb.AppendLine($"======== Arrow to the {ArrowDirection} ========");
            return sb.ToString();
        }
    }
}
