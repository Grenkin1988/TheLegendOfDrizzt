﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Random = System.Random;

namespace Model {
    public class Map {
        private readonly List<Tile> Tiles;

        public Map() {
            Tiles = new List<Tile>();
            InitializeStartTiles();
        }

        private void InitializeStartTiles() {
            var startTile1 = new Tile("Wall;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall");
            startTile1.PlaceTile(0, 0);
            startTile1.RotateTileClockwise();
            startTile1.RotateTileClockwise();
            startTile1.RotateTileClockwise();
            var startTile2 = new Tile("Wall;Floor;Floor;Floor;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall");
            startTile2.PlaceTile(4, 0);
            startTile2.RotateTileClockwise();
            startTile1.SetNeighbor(startTile2, Directions.East);
            startTile2.SetNeighbor(startTile1, Directions.West);
            Tiles.AddRange(new[] { startTile1, startTile2 });
        }

        public ICollection<Tile> GetAllTiles() {
            return new ReadOnlyCollection<Tile>(Tiles);
        }

        public void PlaceNewTileNearExistent(Tile existenTile, Tile newTile, Directions placementDirection) {
            int newTileX = GetNewTileX(existenTile, placementDirection);
            int newTileY = GetNewTileY(existenTile, placementDirection);
            newTile.PlaceTile(newTileX, newTileY);

            RotateTile(newTile, GetNumberOfRotationsNeeded(placementDirection));

            Tiles.Add(newTile);
            SetNeighborsForNewTile(newTile);
            OnNewTileCreated(newTile);
        }

        private void SetNeighborsForNewTile(Tile newTile) {
            int neighborsFound = 0;
            foreach (Tile tile in Tiles) {
                if (neighborsFound >= 4) { break; }
                if (newTile.X + Tile.TileSize == tile.X && newTile.Y == tile.Y) {
                    newTile.SetNeighbor(tile, Directions.East);
                    tile.SetNeighbor(newTile, Directions.West);
                    neighborsFound++;
                    continue;
                }
                if (newTile.X - Tile.TileSize == tile.X && newTile.Y == tile.Y) {
                    newTile.SetNeighbor(tile, Directions.West);
                    tile.SetNeighbor(newTile, Directions.East);
                    neighborsFound++;
                    continue;
                }
                if (newTile.Y + Tile.TileSize == tile.Y && newTile.X == tile.X) {
                    newTile.SetNeighbor(tile, Directions.North);
                    tile.SetNeighbor(newTile, Directions.South);
                    neighborsFound++;
                    continue;
                }
                if (newTile.Y - Tile.TileSize == tile.Y && newTile.X == tile.X) {
                    newTile.SetNeighbor(tile, Directions.South);
                    tile.SetNeighbor(newTile, Directions.North);
                    neighborsFound++;
                }
            }
        }

        public bool TryGetTile(float x, float y, out Tile foundTile) {
            foundTile = Tiles.FirstOrDefault(tile =>
                tile.X <= x &&
                tile.X + Tile.TileSize >= x &&
                tile.Y <= y &&
                tile.Y + Tile.TileSize >= y);
            return foundTile != null;
        }

        public bool IsValidPositionForNewTilePlacement(Tile tile, int x, int y, out Directions? placementDirection) {
            Square square = tile[x, y];
            placementDirection = null;
            if (!IsValidTerrainTypeForNewTilePlacement(square.TerrainType)) { return false; }
            if (!IsValidSquarePositionForNewTilePlacement(x, y)) { return false; }
            placementDirection = GetDirectionForNewTilePlacement(tile, x, y);
            return placementDirection.HasValue;
        }

        private static bool IsValidTerrainTypeForNewTilePlacement(TerrainTypes type) {
            switch (type) {
                case TerrainTypes.Floor:
                case TerrainTypes.Mashrooms:
                return true;
                case TerrainTypes.Wall:
                case TerrainTypes.VolcanicVent:
                return false;
                default:
                throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        private static bool IsValidSquarePositionForNewTilePlacement(int x, int y) {
            return x == 0 || y == 0 || x == Tile.TileSize - 1 || y == Tile.TileSize - 1;
        }

        private static Directions? GetDirectionForNewTilePlacement(Tile tile, int x, int y) {
            Directions? direction = null;
            if (y == 0) { direction = Directions.South; }
            if (x == 0) { direction = Directions.West; }
            if (y == Tile.TileSize - 1) { direction = Directions.North; }
            if (x == Tile.TileSize - 1) { direction = Directions.East; }
            if (x == 0 && y == 0) {
                direction = GetRandomDirectionFromTwo(tile, Directions.South, Directions.West);
            }
            if (x == 0 && y == Tile.TileSize - 1) {
                direction = GetRandomDirectionFromTwo(tile, Directions.West, Directions.North);
            }
            if (x == Tile.TileSize - 1 && y == Tile.TileSize - 1) {
                direction = GetRandomDirectionFromTwo(tile, Directions.North, Directions.East);
            }
            if (x == Tile.TileSize - 1 && y == 0) {
                direction = GetRandomDirectionFromTwo(tile, Directions.East, Directions.South);
            }
            if (direction.HasValue && tile.GetNeighbor(direction.Value) != null) {
                direction = null;
            }
            return direction;
        }

        private static Directions? GetRandomDirectionFromTwo(Tile tile, Directions firstNeighborDirection, Directions secondNeighborDirection) {
            var rnd = new Random();
            int randomValue = rnd.Next(2);
            Tile neighbor1 = tile.GetNeighbor(firstNeighborDirection);
            Tile neighbor2 = tile.GetNeighbor(secondNeighborDirection);
            if (neighbor1 == null && neighbor2 == null) {
                return randomValue == 0 ? firstNeighborDirection : secondNeighborDirection;
            }
            return neighbor1 == null ? firstNeighborDirection : secondNeighborDirection;
        }

        private static int GetNewTileX(Tile existenTile, Directions placementDirection) {
            switch (placementDirection) {
                case Directions.West:
                return existenTile.X - Tile.TileSize;
                case Directions.East:
                return existenTile.X + Tile.TileSize;
                case Directions.South:
                case Directions.North:
                return existenTile.X;
                default:
                throw new ArgumentOutOfRangeException("placementDirection", placementDirection, null);
            }
        }

        private static int GetNewTileY(Tile existenTile, Directions placementDirection) {
            switch (placementDirection) {
                case Directions.South:
                return existenTile.Y - Tile.TileSize;
                case Directions.North:
                return existenTile.Y + Tile.TileSize;
                case Directions.West:
                case Directions.East:
                return existenTile.Y;
                default:
                throw new ArgumentOutOfRangeException("placementDirection", placementDirection, null);
            }
        }

        private static int GetNumberOfRotationsNeeded(Directions placementDirection) {
            switch (placementDirection) {
                case Directions.South:
                return 2;
                case Directions.West:
                return 3;
                case Directions.North:
                return 0;
                case Directions.East:
                return 1;
                default:
                throw new ArgumentOutOfRangeException("placementDirection", placementDirection, null);
            }
        }

        private static void RotateTile(Tile tile, int numberOfRotations) {
            while (numberOfRotations > 0) {
                tile.RotateTileClockwise();
                numberOfRotations--;
            }
        }

        public event Action<Tile> NewTileCreated;

        private void OnNewTileCreated(Tile tile) {
            NewTileCreated?.Invoke(tile);
        }
    }
}