using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Model.PathFinding;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Character {
        private CharacterData _data;

        public string Name { get; }
        public GameObject CharacterGameObject { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Tile CurrentTile { get; private set; }
        public Square CurrentSquare { get; private set; }
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

        public void MoveHere(int x, int y, Tile tile) {
            CurrentTile = tile;
            Square square = tile[x, y];
            CurrentSquare = square;
            X = x + tile.X;
            Y = y + tile.Y;
            CharacterGameObject.transform.position = new Vector3(X, Y, 0);
        }

        public void RecalculatePathfinding(Square[,] squaresMap) {
            BreadthFirstSearch = new BreadthFirstSearch(squaresMap, CurrentSquare, _data.Speed);
            BreadthFirstSearch.LoopSquares();
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
