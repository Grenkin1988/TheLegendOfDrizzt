using System;
using System.Collections.Generic;
using TheLegendOfDrizzt.Assets.Scripts.Utility;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.Condition {
    public class StandNearSquareCondition : WinningConditionBase {
        private Map _adventureMap;
        private List<Coordinates> _coordinatesToCheck = new List<Coordinates>();

        public string RelatedTileName { get; }
        public TerrainTypes Type { get; }
        public int Distanse { get; }

        public StandNearSquareCondition(StandNearSquareConditionData data) {
            RelatedTileName = data.RelatedTileName;
            Type = data.Type;
            Distanse = data.Distanse;
            InitializeCoordinatesToCheck();
        }

        public void SetUpCondition(Map adventureMap) {
            _adventureMap = adventureMap;
        }

        public override bool IsConditionMet(Player player) {
            if (_adventureMap == null) {
                throw new NullReferenceException($"No AdventureMap found. Probably forgot to {nameof(SetUpCondition)}");
            }
            if (!string.IsNullOrEmpty(RelatedTileName)
                && player.Character.CurrentTile.Name != RelatedTileName) {
                return false;
            }
            if (player.Character.CurrentSquare.TerrainType == Type) { return true; }
            Coordinates currentSquareCordinates = _adventureMap.SquaresMap.CoordinatesOf(player.Character.CurrentSquare);
            int x = currentSquareCordinates.X;
            int y = currentSquareCordinates.Y;
            if (x < 0 || y < 0) {
                throw new ArgumentException("Current character square is not in the map");
            }

            foreach (Coordinates coordinates in _coordinatesToCheck) {
                var coordinateToCheck = new Coordinates(coordinates.X + x, coordinates.Y + y);
                if (coordinateToCheck.X < 0
                    || coordinateToCheck.X >= _adventureMap.SquaresMap.GetLength(0)
                    || coordinateToCheck.Y < 0
                    || coordinateToCheck.Y >= _adventureMap.SquaresMap.GetLength(1)) {
                    continue;
                }
                Square squareToCheck = _adventureMap.SquaresMap[coordinateToCheck.X, coordinateToCheck.Y];
                if (squareToCheck?.TerrainType == Type) {
                    return true;
                }
            }

            return false;
        }

        private void InitializeCoordinatesToCheck() {
            for (int x = -Distanse; x <= Distanse; x++) {
                for (int y = -Distanse; y <= Distanse; y++) {
                    _coordinatesToCheck.Add(new Coordinates(x, y));
                }
            }
        }
    }
}
