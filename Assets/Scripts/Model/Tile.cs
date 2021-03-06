﻿using System;
using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Data;
using TheLegendOfDrizzt.Utility;

namespace TheLegendOfDrizzt.Model {
    public class Tile {
        public static readonly int TileSize = 4;
        private readonly Adjacent<Tile> _adjacentTiles;
        private Square[,] _squares;

        private string ID => $"{Name}_{X}_{Y}";
        public string Name { get; }
        public ArrowColor ArrowColor { get; }
        public string Trigger { get; }
        public string Decal { get; }
        public Directions ArrowDirection { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Square this[int x, int y] {
            get {
                if (x >= TileSize || x < 0) { throw new ArgumentOutOfRangeException(nameof(x)); }
                if (y >= TileSize || y < 0) { throw new ArgumentOutOfRangeException(nameof(y)); }
                return _squares[x, y];
            }
        }

        public Tile(TileData tileData) {
            _adjacentTiles = new Adjacent<Tile>();
            _squares = new Square[TileSize, TileSize];
            Name = tileData.Name;
            ArrowColor = tileData.ArrowColor;
            Trigger = tileData.Trigger;
            Decal = tileData.Decal;
            FillTileByLayout(tileData.Layout);
        }

        public void PlaceTile(int x, int y) {
            X = x;
            Y = y;
        }

        public void RotateTileClockwise() {
            var newArray = MathUtility.RotateArrayClockwise(_squares, TileSize);
            int newArrowDirection = (int)ArrowDirection + 1;
            if (newArrowDirection > 4) { newArrowDirection -= 4; }
            ArrowDirection = (Directions)newArrowDirection;
            _squares = newArray;
        }

        public void RotateTileCounterClockwise() {
            var newArray = MathUtility.RotateArrayCounterClockwise(_squares, TileSize);
            int newArrowDirection = (int)ArrowDirection - 1;
            if (newArrowDirection <= 0) { newArrowDirection += 4; }
            ArrowDirection = (Directions)newArrowDirection;
            _squares = newArray;
        }

        public void SetNeighbor(Tile tile, Directions direction) {
            _adjacentTiles.SetAdjacent(direction, tile);
        }

        public Tile GetNeighbor(Directions direction) {
            var adjacent = _adjacentTiles.GetAdjacent(direction);
            return adjacent != null && adjacent.Any() ? adjacent.First() : null;
        }

        public bool CanMoveHere(int x, int y) {
            var square = _squares[x, y];
            return square.CanMoveHere();
        }

        public override string ToString() => ID;

        public Coordinates? FindSquareCoordinates(Square square) {
            for (int x = 0; x < TileSize; x++) {
                for (int y = 0; y < TileSize; y++) {
                    if (_squares[x, y] == square) {
                        return new Coordinates(x, y);
                    }
                }
            }
            return null;
        }
        
        private void FillTileByLayout(string squaresSequence) {
            string[] splittedSequence = squaresSequence.Split(';');
            if (splittedSequence.Length != TileSize * TileSize) {
                throw new InvalidOperationException($"Squares sequence must contain {TileSize * TileSize} squares");
            }
            int splittedSequenceIndex = 0;
            for (int x = 0; x < TileSize; x++) {
                for (int y = 0; y < TileSize; y++) {
                    string terrainTypeText = splittedSequence[splittedSequenceIndex];
                    var terrain = Enums.ParceEnumValue<TerrainTypes>(terrainTypeText);
                    _squares[x, y] = new Square(terrain, this);
                    splittedSequenceIndex++;
                }
            }
            ArrowDirection = Directions.South;
        }
    }
}
