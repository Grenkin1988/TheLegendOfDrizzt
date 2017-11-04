using System.Collections.Generic;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using UnityEngine;

namespace Model {
    public class TilesDeck {
        private readonly Queue<TileData> Deck;

        public TilesDeck() {
            TilesLibrary.InitializeLibrary();
            Deck = new Queue<TileData>(TilesLibrary.DefaultTiles);
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
