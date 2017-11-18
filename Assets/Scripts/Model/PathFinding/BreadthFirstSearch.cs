using System.Collections.Generic;
using System.Linq;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.PathFinding {
    public class BreadthFirstSearch {
        private static readonly Coordinates[] _prioritizedDirection = {
            new Coordinates(0, 1),
            new Coordinates(0, -1),
            new Coordinates(-1, 0),
            new Coordinates(1, 0),
            new Coordinates(-1, 1, true),
            new Coordinates(1, 1, true),
            new Coordinates(-1, -1, true),
            new Coordinates(1, -1, true)
        };

        private Queue<Square> _frontire = new Queue<Square>();
        private Dictionary<Square, bool> _visited = new Dictionary<Square, bool>();
        private Square[,] _squaresMap;
        private int _maxMovement;

        private Square StartingSquare { get; }

        public IReadOnlyCollection<Square> ReachableSquares => 
            _visited.Keys
            .Where(square => square.DistanceFromStart <= _maxMovement)
            .ToList();

        public BreadthFirstSearch(Square[,] squares, Square startingSquare, int maxMovement) {
            _squaresMap = squares;
            StartingSquare = startingSquare;
            _maxMovement = maxMovement;
            StartingSquare.DistanceFromStart = 0;
            _frontire.Enqueue(StartingSquare);
            _visited[StartingSquare] = true;
        }

        public void LoopSquares() {
            while (_frontire.Count != 0) {
                Square current = _frontire.Dequeue();
                foreach (Square neighbor in GetNeighbors(_squaresMap, current)) {
                    if (_visited.ContainsKey(neighbor)
                        || current.DistanceFromStart >= _maxMovement) { continue; }
                    neighbor.DistanceFromStart = current.DistanceFromStart + 1;
                    _frontire.Enqueue(neighbor);
                    _visited[neighbor] = true;
                }
            }
        }

        public Square[] GetPathTo(Square target) {
            if (target.DistanceFromStart == null) { return new Square[0]; }
            var path = new List<Square> {
                target
            };
            Square current = target;
            while (current != StartingSquare) {
                foreach (Square neighbor in GetNeighbors(_squaresMap, current)) {
                    if (neighbor.DistanceFromStart < current.DistanceFromStart) {
                        current = neighbor;
                        path.Add(current);
                        break;
                    }
                }
            }
            return path.ToArray();
        }

        private static Square[] GetNeighbors(Square[,] squares, Square square) {
            int maxX = squares.GetLength(0);
            int maxY = squares.GetLength(1);

            var neighbours = new List<Square>();
            var walls = new List<Square>();
            foreach (Coordinates coordinates in _prioritizedDirection) {
                int xToCheck = square.MapCoordinates.X + coordinates.X;
                int yToCheck = square.MapCoordinates.Y + coordinates.Y;
                if (xToCheck < 0 || xToCheck >= maxX) { continue; }
                if (yToCheck < 0 || yToCheck >= maxY) { continue; }

                Square nextSquare = squares[xToCheck, yToCheck];

                if (nextSquare == null
                    || nextSquare.TerrainType.CannotMoveOnHere()) {
                    walls.Add(nextSquare);
                    continue;
                }

                if (coordinates.IsDiagonal && walls.Count >= 2) {
                    if (squares[xToCheck - coordinates.X, yToCheck].TerrainType.CannotMoveOnHere()
                        && squares[xToCheck, yToCheck - coordinates.Y].TerrainType.CannotMoveOnHere()) {
                        continue;
                    }
                }

                neighbours.Add(nextSquare);
            }
            return neighbours.ToArray();
        }
    }
}
