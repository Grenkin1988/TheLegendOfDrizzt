using System.Collections.Generic;
using TheLegendOfDrizzt.Controller;
using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.View {
    public class TileView {
        private readonly Tile _tile;
        private readonly Transform _parentTransform;
        private readonly SquareView[,] _squareViews;
        private readonly Dictionary<Square, SquareView> _squareViewModels = new Dictionary<Square, SquareView>();

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

            DrawArrow();

            if (!string.IsNullOrEmpty(_tile.Decal)) {
                DrawDecal();
            }

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    var square = _tile[x, y];
                    if (square == null) { continue; }
                    var squareView = new SquareView(square, TileGameObject.transform);
                    squareView.Draw(x, y, _tile.ArrowDirection);
                    _squareViews[x, y] = squareView;
                    _squareViewModels[square] = squareView;
                }
            }
        }

        public SquareView GetSquareViewForSquare(Square square) {
            _squareViewModels.TryGetValue(square, out var squareView);
            return squareView;
        }

        private void DrawArrow() {
            var arrow = new GameObject("Arrow");
            arrow.transform.position = new Vector3(2, 2, 0);
            arrow.transform.Rotate(0, 0, 90 * SpriteManager.GetNumberOfSpriteRotationsNeeded(_tile.ArrowDirection));
            arrow.transform.SetParent(TileGameObject.transform, false);
            var arrowRenderer = arrow.AddComponent<SpriteRenderer>();
            string tileTypeText = $"Arrow{_tile.ArrowColor}";
            arrowRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileTypeText);
            arrowRenderer.sortingLayerName = "Decal";
        }

        private void DrawDecal() {
            var arrow = new GameObject("Decal");
            arrow.transform.position = new Vector3(2, 2, 0);
            arrow.transform.Rotate(0, 0, 90 * SpriteManager.GetNumberOfSpriteRotationsNeeded(_tile.ArrowDirection));
            arrow.transform.SetParent(TileGameObject.transform, false);
            var tileRenderer = arrow.AddComponent<SpriteRenderer>();
            tileRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(_tile.Decal);
            tileRenderer.sortingLayerName = "Decal";
        }
    }
}
