using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.View {
    public class SquareView {
        private readonly Square _square;
        private readonly Transform _parentTransform;

        public GameObject SquareGameObject { get; private set; }

        public SquareView(Square square, Transform parentTransform) {
            _parentTransform = parentTransform;
            _square = square;
        }

        public void Draw(int x, int y, Directions placementDirection) {
            float newX = x + 0.5f;
            float newY = y + 0.5f;
            SquareGameObject = new GameObject("Square_" + x + "_" + y);
            SquareGameObject.transform.position = new Vector3(newX, newY, 0);
            SquareGameObject.transform.Rotate(
                0, 
                0, 
                90 * SpriteManager.GetNumberOfTileSpriteRotationsNeeded(_square.TerrainType, placementDirection));
            SquareGameObject.transform.SetParent(_parentTransform, false);
            SquareGameObject.AddComponent<BoxCollider2D>();
            var squareRenderer = SquareGameObject.AddComponent<SpriteRenderer>();
            string tileTypeText = _square.TerrainType.ToString();
            squareRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileTypeText);
            squareRenderer.sortingLayerName = "Tiles";
        }
    }
}
