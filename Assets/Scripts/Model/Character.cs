using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Character {
        public string Name { get; }
        public GameObject CharacterGameObject { get; private set; }
        public Tile CurrentTile { get; private set; }
        public Square CurrentSquare { get; private set; }

        public Character(CharacterData data) {
            Name = data.Name;
            InitializeCharacter();
        }

        public void Place(int x, int y) {
            CharacterGameObject.transform.position = new Vector3(x, y, 0);
            if (!CharacterGameObject.activeSelf) {
                CharacterGameObject.SetActive(true);
            }
        }

        public void MoveHere(int x, int y, Tile tile) {
            CurrentTile = tile;
            Square square = tile[x, y];
            CurrentSquare = square;
            CharacterGameObject.transform.position = new Vector3(x + tile.X, y + tile.Y, 0);
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
