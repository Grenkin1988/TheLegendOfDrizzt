using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class SpriteManager {
        private static SpriteManager _instance;
        private static readonly object _padlock = new object();
        private const string TILE_SPRITE_PATH = "Images/Tiles";
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

        private readonly Random _random = new Random();
        private readonly Dictionary<string, List<Sprite>> _spritesMap = new Dictionary<string, List<Sprite>>();

        public IReadOnlyDictionary<string, List<Sprite>> SpritesMap => new ReadOnlyDictionary<string, List<Sprite>>(_spritesMap);

        private SpriteManager() {
            LoadSprites();
        }

        public Sprite LoadSpriteByName(string name) {
            if (_spritesMap.ContainsKey(name)) {
                List<Sprite> list = _spritesMap[name];
                int index = 0;
                if (list.Count > 1) {
                    index = _random.Next(list.Count);
                }
                return _spritesMap[name][index];
            }
            return _spritesMap[DEFAULT_SPRITE_NAME].First();
        }

        private void LoadSprites() {
            Sprite[] sprites = Resources.LoadAll<Sprite>(TILE_SPRITE_PATH);
            foreach (Sprite sprite in sprites) {
                string name = GetSpriteName(sprite);
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
    }
}
