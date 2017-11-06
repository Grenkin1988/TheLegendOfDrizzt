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

        public void Draw(int x, int y) {
            SquareGameObject = new GameObject("Square_" + x + "_" + y);
            SquareGameObject.transform.position = new Vector3(x, y, 0);
            SquareGameObject.transform.SetParent(_parentTransform, false);
            var boxCollier2D = SquareGameObject.AddComponent<BoxCollider2D>();
            boxCollier2D.offset = new Vector2(0.5f, 0.5f);
            var squareRenderer = SquareGameObject.AddComponent<SpriteRenderer>();
            string tileTypeText = _square.TerrainType.ToString();
            squareRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileTypeText);
            squareRenderer.sortingLayerName = "Tiles";
        }
    }
}
