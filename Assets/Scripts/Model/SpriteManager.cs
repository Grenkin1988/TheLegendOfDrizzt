using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class SpriteManager {
        private static SpriteManager _instance;
        private static readonly object _padlock = new object();
        private const string TILE_SPRITE_PATH = "Images/Tiles";
        private const string DEFAULT_SPRITE_NAME = "Default";

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

        private readonly Dictionary<string, Sprite> _spritesMap = new Dictionary<string, Sprite>();

        public IReadOnlyDictionary<string, Sprite> SpritesMap => new ReadOnlyDictionary<string, Sprite>(_spritesMap);

        private SpriteManager() {
            LoadSprites();
        }

        public Sprite LoadSpriteByName(string name) {
            if (_spritesMap.ContainsKey(name)) {
                return _spritesMap[name];
            }
            return _spritesMap[DEFAULT_SPRITE_NAME];
        }

        private void LoadSprites() {
            Sprite[] sprites = Resources.LoadAll<Sprite>(TILE_SPRITE_PATH);
            foreach (Sprite sprite in sprites) {
                _spritesMap[sprite.name] = sprite;
            }
        }
    }
}
