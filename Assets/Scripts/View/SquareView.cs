using TheLegendOfDrizzt.Controller;
using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.View {
    public class SquareView {
        private readonly Square _square;
        private readonly Transform _parentTransform;

        private GameObject _squareGameObject;

        public SpriteRenderer SquareRenderer { get; private set; }

        public SquareView(Square square, Transform parentTransform) {
            _parentTransform = parentTransform;
            _square = square;
        }

        public void Draw(int x, int y, Directions placementDirection) {
            float newX = x + 0.5f;
            float newY = y + 0.5f;
            _squareGameObject = new GameObject("Square_" + x + "_" + y);
            _squareGameObject.transform.position = new Vector3(newX, newY, 0);
            _squareGameObject.transform.Rotate(
                0, 
                0, 
                90 * SpriteManager.GetNumberOfTileSpriteRotationsNeeded(_square.TerrainType, placementDirection));
            _squareGameObject.transform.SetParent(_parentTransform, false);
            _squareGameObject.AddComponent<BoxCollider2D>();
            SquareRenderer = _squareGameObject.AddComponent<SpriteRenderer>();
            string tileTypeText = _square.TerrainType.ToString();
            SquareRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileTypeText);
            SquareRenderer.sortingLayerName = "Tiles";
        }
    }
}
