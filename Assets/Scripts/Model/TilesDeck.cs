using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Data;

namespace Model {
    public class TilesDeck {
        public Tile GetNexTile() {
            return new Tile(TilesLibrary.DefaultTiles.First().Layout);
        } 
    }
}
