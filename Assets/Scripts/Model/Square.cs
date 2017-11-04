//=======================================================================
// Copyright Glib "Grenkin" Kruglov 2016.
//=======================================================================

namespace Model {
    public class Square {
        public TerrainTypes TerrainType { get; private set; }

        public Square(TerrainTypes terrainType) {
            TerrainType = terrainType;
        }

        public override string ToString() {
            return string.Format("Terrain: {0}", TerrainType);
        }
    }
}