using System;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Square {
        public TerrainTypes TerrainType { get; private set; }

        public Square(TerrainTypes terrainType) {
            TerrainType = terrainType;
        }

        public override string ToString() {
            return $"Terrain: {TerrainType}";
        }

        public bool CanMoveHere() {
            return TerrainType.CanMoveOnHere();
        }
    }
}
