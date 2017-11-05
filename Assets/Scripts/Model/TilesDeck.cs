using System.Collections.Generic;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using UnityEngine;

namespace Model {
    public class TilesDeck {
        private readonly Queue<TileData> Deck;
        private readonly TilesLibrary TilesLibrary;

        public TilesDeck() {
            TilesLibrary = TilesLibrary.Instance;
            Deck = new Queue<TileData>(TilesLibrary.SimpleTiles.Values);
        }

        public Tile GetNexTile() {
            if (Deck.Count != 0) {
                return new Tile(Deck.Dequeue());
            } else {
                Debug.Log("No more Tiles");
                return null;
            }
        } 
    }
}
