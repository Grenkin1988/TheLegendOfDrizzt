using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Model.PathFinding;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Character {
        private CharacterData _data;
        public GameObject _characterPathGameObject;

        public string Name { get; }
        public GameObject CharacterGameObject { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Tile CurrentTile { get; private set; }
        public Square CurrentSquare { get; private set; }
        public Tile MovementTargetTile { get; private set; }
        public Square MovementTargetSquare { get; private set; }
        public BreadthFirstSearch BreadthFirstSearch { get; private set; }

        public Character(CharacterData data) {
            _data = data;
            Name = data.Name;
            InitializeCharacter();
        }

        public void Place(int x, int y, Tile tile) {
            MoveHere(x, y, tile);
            if (!CharacterGameObject.activeSelf) {
                CharacterGameObject.SetActive(true);
            }
        }

        public bool MoveToTarget(int x, int y, Tile tile) {
            if(tile != MovementTargetTile) { return false; }
            Square square = tile[x, y];
            if (square != MovementTargetSquare) { return false; }
            CurrentTile = tile;
            CurrentSquare = square;
            X = x + tile.X;
            Y = y + tile.Y;
            ResetPath();
            CharacterGameObject.transform.position = new Vector3(X + 0.5f, Y + 0.5f, 0);
            return true;
        }

        public void RecalculatePathfinding(Square[,] squaresMap) {
            BreadthFirstSearch = new BreadthFirstSearch(squaresMap, CurrentSquare, _data.Speed);
            BreadthFirstSearch.LoopSquares();
        }

        public bool UpdateMovementTarget(int x, int y, Tile tile) {
            MovementTargetTile = tile;
            Square square = tile[x, y];
            if(MovementTargetSquare == square 
                || CurrentSquare == square) { return false;}
            MovementTargetSquare = square;
            return true;
        }

        public void UpdatePath() {
            DestroyPath();
            if (MovementTargetSquare != null) {
                DrawPath();
            }
        }

        public void ResetPath() {
            MovementTargetTile = null;
            MovementTargetSquare = null;
            DestroyPath();
        }

        private void DestroyPath() {
            Object.Destroy(_characterPathGameObject);
        }

        private void MoveHere(int x, int y, Tile tile) {
            CurrentTile = tile;
            Square square = tile[x, y];
            CurrentSquare = square;
            X = x + tile.X;
            Y = y + tile.Y;
            ResetPath();
            CharacterGameObject.transform.position = new Vector3(X + 0.5f, Y + 0.5f, 0);
        }

        private void DrawPath() {
            Coordinates? coordinates = MovementTargetTile.FindSquareCoordinates(MovementTargetSquare);
            if (coordinates == null) { return; }
            float x = coordinates.Value.X + MovementTargetTile.X + 0.5f;
            float y = coordinates.Value.Y + MovementTargetTile.Y + 0.5f;

            _characterPathGameObject = new GameObject("Path");
            _characterPathGameObject.transform.SetParent(CharacterGameObject.transform);
            var targetGameObject = new GameObject($"Target_{MovementTargetSquare.MapCoordinates.X}_{MovementTargetSquare.MapCoordinates.Y}");
            targetGameObject.transform.position = new Vector3(x, y, 0);
            targetGameObject.transform.SetParent(_characterPathGameObject.transform);
            var characterRenderer = targetGameObject.AddComponent<SpriteRenderer>();
            characterRenderer.sprite = SpriteManager.Instance.LoadSpriteByName("travel_path_target");
            characterRenderer.sortingLayerName = "PathTarget";
        }

        private void InitializeCharacter() {
            CharacterGameObject = new GameObject(Name);
            CharacterGameObject.transform.SetParent(GameObject.Find("Players").transform);
            CharacterGameObject.AddComponent<Controller.CharacterController>();
            var characterRenderer = CharacterGameObject.AddComponent<SpriteRenderer>();
            characterRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(Name);
            characterRenderer.sortingLayerName = "Player";
            CharacterGameObject.SetActive(false);
        }
    }
}
