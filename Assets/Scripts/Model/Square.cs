namespace Model {
    public class Square {
        public TerrainTypes TerrainType { get; private set; }

        public Square(TerrainTypes terrainType) {
            TerrainType = terrainType;
        }

        public override string ToString() {
            return $"Terrain: {TerrainType}";
        }
    }
}
