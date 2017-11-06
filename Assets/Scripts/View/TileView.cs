using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.View {
    public class TileView {
        private readonly Tile _tile;
        private readonly Transform _parentTransform;
        private SquareView[,] _squareViews;

        public GameObject TileGameObject { get; private set; }

        public TileView(Tile tile, Transform parentTransform) {
            _tile = tile;
            _parentTransform = parentTransform;
            _squareViews = new SquareView[Tile.TileSize, Tile.TileSize];
        }

        public void Draw() {
            TileGameObject = new GameObject($"Tile_{_tile.X}_{_tile.Y}-{_tile.Name}");
            TileGameObject.transform.position = new Vector3(_tile.X, _tile.Y, 0);
            TileGameObject.transform.SetParent(_parentTransform, true);

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    var squareView = new SquareView(_tile[x, y], TileGameObject.transform);
                    squareView.Draw(x, y);
                    _squareViews[x, y] = squareView;
                }
            }
        }
    }
}
