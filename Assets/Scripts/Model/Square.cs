
namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Square {
        public Tile ParentTile { get; }
        public Coordinates MapCoordinates { get; set; }
        public TerrainTypes TerrainType { get; }
        public int? DistanceFromStart { get; set; }

        public Square(TerrainTypes terrainType, Tile parentTile) {
            TerrainType = terrainType;
            ParentTile = parentTile;
        }

        public override string ToString() 
            => $"X: {MapCoordinates.X}, Y: {MapCoordinates.Y} - Terrain: {TerrainType} [{DistanceFromStart}]";

        public bool CanMoveHere() {
            return DistanceFromStart.HasValue
                && TerrainType.CanMoveOnHere();
        }
    }
}
