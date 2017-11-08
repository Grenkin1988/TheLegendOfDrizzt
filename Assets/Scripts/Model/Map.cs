﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using UnityEngine;
using Random = System.Random;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Map {
        private const string START_TILE_NAME = "StartTile";
        private readonly Dictionary<string, Tile> Tiles;
        private readonly TilesLibrary TilesLibrary;
        private readonly Random _random = new Random();
        private readonly List<Vector2> _startingPositions = new List<Vector2> {
            new Vector2(1, 1),
            new Vector2(1, 2),
            new Vector2(2, 1),
            new Vector2(3, 1),
            new Vector2(3, 2)
        };

        public Map() {
            Tiles = new Dictionary<string, Tile>();
            TilesLibrary = TilesLibrary.Instance;
            InitializeStartTiles();
        }

        public IReadOnlyDictionary<string, Tile> GetAllTiles() {
            return new ReadOnlyDictionary<string, Tile>(Tiles);
        }

        public void PlaceNewTileNearExistent(Tile existenTile, Tile newTile, Directions placementDirection) {
            int newTileX = GetNewTileX(existenTile, placementDirection);
            int newTileY = GetNewTileY(existenTile, placementDirection);
            newTile.PlaceTile(newTileX, newTileY);

            RotateTile(newTile, GetNumberOfRotationsNeeded(placementDirection));

            Tiles.Add(newTile.Name, newTile);
            SetNeighborsForNewTile(newTile);
            OnNewTileCreated(newTile);
        }

        public bool TryGetTile(float x, float y, out Tile foundTile) {
            foundTile = Tiles.Values.FirstOrDefault(tile =>
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

        public void SetStartingPlayersPosition(params Player[] players) {
            foreach (Player player in players) {
                int positionIndex = _random.Next(0, _startingPositions.Count);
                Vector2 position = _startingPositions[positionIndex];
                player.Character.Place((int)position.x, (int)position.y, Tiles["StartTile_1"]);
                _startingPositions.RemoveAt(positionIndex);
            }
        }

        public void PlaceDoubleTileToTileWithName(string doubleTileNeme, string tileName) {
            tileName = tileName.Split('-', '_').First();
            if (!Tiles.ContainsKey(tileName)) {
                throw new ArgumentException($"There is no tile ({tileName}) in Map");
            }
            Tile tile = Tiles[tileName];
            Tile[] dubleTile = TilesLibrary.GetDoubleTile(doubleTileNeme).Select(data => new Tile(data)).ToArray();
            if (dubleTile == null || dubleTile.Length != 2) {
                throw new ApplicationException($"Double tile ({doubleTileNeme}) not found, or wrong quantity");
            }
            PlaceNewTileNearExistent(tile, dubleTile[0], tile.ArrowDirection.Oposite());
            PlaceNewTileNearExistent(dubleTile[0], dubleTile[1], dubleTile[0].ArrowDirection.Oposite());
        }

        private void InitializeStartTiles() {
            TileData[] startingTiles = TilesLibrary.GetDoubleTile(START_TILE_NAME);
            if (startingTiles == null || startingTiles.Length != 2) {
                throw new ApplicationException("Starting tiles not found, or wrong quantity");
            }
            var startTile1 = new Tile(startingTiles[0]);
            startTile1.PlaceTile(0, 0);
            startTile1.RotateTileCounterClockwise();
            var startTile2 = new Tile(startingTiles[1]);
            startTile2.PlaceTile(4, 0);
            startTile2.RotateTileClockwise();
            startTile1.SetNeighbor(startTile2, Directions.East);
            startTile2.SetNeighbor(startTile1, Directions.West);
            Tiles[startTile1.Name] = startTile1;
            Tiles[startTile2.Name] = startTile2;
        }

        private void SetNeighborsForNewTile(Tile newTile) {
            int neighborsFound = 0;
            foreach (Tile tile in Tiles.Values) {
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

        private static bool IsValidTerrainTypeForNewTilePlacement(TerrainTypes type) {
            switch (type) {
                case TerrainTypes.Floor:
                case TerrainTypes.Mashrooms:
                case TerrainTypes.Crystal:
                case TerrainTypes.Bridge:
                case TerrainTypes.Lair:
                    return true;
                default:
                    return false;
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
                    throw new ArgumentOutOfRangeException(nameof(placementDirection), placementDirection, null);
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
                    throw new ArgumentOutOfRangeException(nameof(placementDirection), placementDirection, null);
            }
        }

        private static int GetNumberOfRotationsNeeded(Directions placementDirection) {
            switch (placementDirection) {
                case Directions.South:
                    return 2;
                case Directions.West:
                    return -1;
                case Directions.North:
                    return 0;
                case Directions.East:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(placementDirection), placementDirection, null);
            }
        }

        private static void RotateTile(Tile tile, int numberOfRotations) {
            if (numberOfRotations < 0) {
                tile.RotateTileCounterClockwise();
            } else {
                while (numberOfRotations > 0) {
                    tile.RotateTileClockwise();
                    numberOfRotations--;
                }
            }
        }

        public event Action<Tile> NewTileCreated;

        private void OnNewTileCreated(Tile tile) {
            NewTileCreated?.Invoke(tile);
        }
    }
}
