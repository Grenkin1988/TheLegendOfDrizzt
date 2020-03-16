using System;
using System.Collections.Generic;
using TheLegendOfDrizzt.Data.Condition;
using TheLegendOfDrizzt.Utility;

namespace TheLegendOfDrizzt.Model.Condition {
    public class StandNearSquareCondition : WinningConditionBase {
        private Map _adventureMap;
        private readonly List<Coordinates> _coordinatesToCheck = new List<Coordinates>();
        private readonly string _relatedTileName;
        private readonly TerrainTypes _type;
        private readonly int _distanse;

        public StandNearSquareCondition(StandNearSquareConditionData data) {
            _relatedTileName = data.RelatedTileName;
            _type = data.Type;
            _distanse = data.Distanse;
            InitializeCoordinatesToCheck();
        }

        public void SetUpCondition(Map adventureMap) {
            _adventureMap = adventureMap;
        }

        public override bool IsConditionMet(Player player) {
            if (_adventureMap == null) {
                throw new NullReferenceException($"No AdventureMap found. Probably forgot to {nameof(SetUpCondition)}");
            }
            if (!string.IsNullOrEmpty(_relatedTileName)
                && player.Character.CurrentTile.Name != _relatedTileName) {
                return false;
            }
            if (player.Character.CurrentSquare.TerrainType == _type) { return true; }
            var currentSquareCordinates = _adventureMap.SquaresMap.CoordinatesOf(player.Character.CurrentSquare);
            int x = currentSquareCordinates.X;
            int y = currentSquareCordinates.Y;
            if (x < 0 || y < 0) {
                throw new ArgumentException("Current character square is not in the map");
            }

            foreach (var coordinates in _coordinatesToCheck) {
                var coordinateToCheck = new Coordinates(coordinates.X + x, coordinates.Y + y);
                if (coordinateToCheck.X < 0
                    || coordinateToCheck.X >= _adventureMap.SquaresMap.GetLength(0)
                    || coordinateToCheck.Y < 0
                    || coordinateToCheck.Y >= _adventureMap.SquaresMap.GetLength(1)) {
                    continue;
                }
                var squareToCheck = _adventureMap.SquaresMap[coordinateToCheck.X, coordinateToCheck.Y];
                if (squareToCheck?.TerrainType == _type) {
                    return true;
                }
            }

            return false;
        }

        private void InitializeCoordinatesToCheck() {
            for (int x = -_distanse; x <= _distanse; x++) {
                for (int y = -_distanse; y <= _distanse; y++) {
                    _coordinatesToCheck.Add(new Coordinates(x, y));
                }
            }
        }
    }
}
