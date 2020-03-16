using TheLegendOfDrizzt.Data;
using TheLegendOfDrizzt.Model.PathFinding;
using TheLegendOfDrizzt.View;

namespace TheLegendOfDrizzt.Model {
    public class Character {
        private readonly CharacterData _data;
        private CharacterView _characterView;

        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Tile CurrentTile { get; private set; }
        public Square CurrentSquare { get; private set; }
        public Tile MovementTargetTile { get; private set; }
        public Square MovementTargetSquare { get; private set; }
        public BreadthFirstSearch BreadthFirstSearch { get; private set; }
        public Square[] PathToTarget { get; private set; }

        public Character(CharacterData data) {
            _data = data;
            Name = data.Name;
            InitializeCharacter();
        }

        public void SetStartingPosition(int x, int y, Tile tile) {
            MoveHere(x, y, tile, true);
            _characterView.Show();
        }

        public bool MoveToTarget(int x, int y, Tile tile) {
            if (tile != MovementTargetTile) { return false; }
            var square = tile[x, y];
            if (square != MovementTargetSquare) { return false; }
            MoveHere(x, y, tile);
            return true;
        }

        public void RecalculatePathfinding(Square[,] squaresMap) {
            BreadthFirstSearch = new BreadthFirstSearch(squaresMap, CurrentSquare, _data.Speed);
            BreadthFirstSearch.LoopSquares();
        }

        public bool UpdateMovementTarget(int x, int y, Tile tile) {
            MovementTargetTile = tile;
            var square = tile[x, y];
            if (MovementTargetSquare == square
                || CurrentSquare == square) { return false; }
            MovementTargetSquare = square;
            PathToTarget = BreadthFirstSearch.GetPathTo(MovementTargetSquare);
            return PathToTarget != null && PathToTarget.Length != 0;
        }

        public void UpdatePath() {
            _characterView.DestroyPath();
            if (MovementTargetSquare != null) {
                _characterView.DrawPath();
            }
        }

        public void ResetPath() {
            MovementTargetTile = null;
            MovementTargetSquare = null;
            _characterView.DestroyPath();
        }

        private void MoveHere(int x, int y, Tile tile, bool isInitialPlace = false) {
            CurrentTile = tile;
            var square = tile[x, y];
            CurrentSquare = square;
            X = x + tile.X;
            Y = y + tile.Y;
            ResetPath();
            _characterView.MoveTo(isInitialPlace);
        }

        private void InitializeCharacter() {
            _characterView = new CharacterView(this);
        }
    }
}
