﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TheLegendOfDrizzt.Model;
using UnityEngine;
using Random = System.Random;

namespace TheLegendOfDrizzt.Controller {
    public class SpriteManager {
        private static SpriteManager _instance;
        private static readonly object _padlock = new object();
        private static readonly Random _random = new Random();
        private const string TILE_SPRITE_PATH = "Images/Tiles";
        private const string DECAL_SPRITE_PATH = "Images/Decals";
        private const string CHARACTERS_SPRITE_PATH = "Images/Characters";
        private const string PATH_SPRITE_PATH = "Images/Path";
        private const string DEFAULT_SPRITE_NAME = "Default";
        private const char NAME_SEPARATOR = '_';

        public static SpriteManager Instance {
            get {
                if (_instance == null) {
                    lock (_padlock) {
                        if (_instance == null) {
                            _instance = new SpriteManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, List<Sprite>> _spritesMap = new Dictionary<string, List<Sprite>>();

        public IReadOnlyDictionary<string, List<Sprite>> SpritesMap => new ReadOnlyDictionary<string, List<Sprite>>(_spritesMap);

        private SpriteManager() {
            LoadTileSprites();
            LoadDecalSprites();
            LoadCharacterSprites();
            LoadPathSprites();
        }

        public Sprite LoadSpriteByName(string name) {
            if (_spritesMap.ContainsKey(name)) {
                var list = _spritesMap[name];
                int index = 0;
                if (list.Count > 1) {
                    index = _random.Next(list.Count);
                }
                return _spritesMap[name][index];
            }
            return _spritesMap[DEFAULT_SPRITE_NAME].First();
        }

        private void LoadTileSprites() {
            var sprites = Resources.LoadAll<Sprite>(TILE_SPRITE_PATH);
            foreach (var sprite in sprites) {
                string name = GetSpriteName(sprite);
                if (!_spritesMap.ContainsKey(name)) {
                    _spritesMap[name] = new List<Sprite>();
                }
                _spritesMap[name].Add(sprite);
            }
        }

        private void LoadDecalSprites() {
            var sprites = Resources.LoadAll<Sprite>(DECAL_SPRITE_PATH);
            foreach (var sprite in sprites) {
                string name = GetSpriteName(sprite);
                if (!_spritesMap.ContainsKey(name)) {
                    _spritesMap[name] = new List<Sprite>();
                }
                _spritesMap[name].Add(sprite);
            }
        }

        private void LoadCharacterSprites() {
            var sprites = Resources.LoadAll<Sprite>(CHARACTERS_SPRITE_PATH);
            foreach (Sprite sprite in sprites) {
                string name = GetSpriteName(sprite);
                if (!_spritesMap.ContainsKey(name)) {
                    _spritesMap[name] = new List<Sprite>();
                }
                _spritesMap[name].Add(sprite);
            }
        }

        private void LoadPathSprites() {
            var sprites = Resources.LoadAll<Sprite>(PATH_SPRITE_PATH);
            foreach (Sprite sprite in sprites) {
                string name = sprite.name;
                if (!_spritesMap.ContainsKey(name)) {
                    _spritesMap[name] = new List<Sprite>();
                }
                _spritesMap[name].Add(sprite);
            }
        }

        private string GetSpriteName(Sprite sprite) {
            if (sprite.name.Contains(NAME_SEPARATOR)) {
                return sprite.name.Split(NAME_SEPARATOR)[0];
            }
            return sprite.name;
        }

        public static int GetNumberOfTileSpriteRotationsNeeded(TerrainTypes type, Directions placementDirection) {
            switch (type) {
                case TerrainTypes.Floor:               
                case TerrainTypes.Mashrooms:
                case TerrainTypes.Chasm:
                case TerrainTypes.Pillar:
                case TerrainTypes.River:
                case TerrainTypes.Lair: {
                    return GetRandomSpriteRotation();
                }

                case TerrainTypes.VolcanicVent:
                case TerrainTypes.Bridge:
                case TerrainTypes.Crystal:               
                case TerrainTypes.Throne: {
                    return GetNumberOfSpriteRotationsNeeded(placementDirection);
                }

                case TerrainTypes.DwarfStatue:
                case TerrainTypes.Wall:
                case TerrainTypes.Campfire:
                case TerrainTypes.Exit: {
                    return 0;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static int GetNumberOfSpriteRotationsNeeded(Directions placementDirection) {
            switch (placementDirection) {
                case Directions.South:
                    return 0;
                case Directions.West:
                    return -1;
                case Directions.North:
                    return 2;
                case Directions.East:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(placementDirection), placementDirection, null);
            }
        }

        private static int GetRandomSpriteRotation() {
            return _random.Next(-1, 3);
        }
    }
}
