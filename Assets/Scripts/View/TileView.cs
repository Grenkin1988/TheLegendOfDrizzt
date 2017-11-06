using System;
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
            var arrow = new GameObject("Arrow");
            arrow.transform.position = new Vector3(2, 2, 0);
            arrow.transform.Rotate(0, 0, 90 * GetNumberOfRotationsNeeded(_tile.ArrowDirection));
            arrow.transform.SetParent(TileGameObject.transform, false);
            var tileRenderer = arrow.AddComponent<SpriteRenderer>();
            string tileTypeText = $"Arrow{_tile.ArrowColor.ToString()}";
            tileRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileTypeText);
            tileRenderer.sortingLayerName = "Decal";

            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < 4; y++) {
                    var squareView = new SquareView(_tile[x, y], TileGameObject.transform);
                    squareView.Draw(x, y);
                    _squareViews[x, y] = squareView;
                }
            }
        }

        private static int GetNumberOfRotationsNeeded(Directions placementDirection) {
            switch (placementDirection) {
                case Directions.South:
                    return 0;
                case Directions.West:
                    return -1;
                case Directions.North:
                    return 2;
                case Directions.East:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(placementDirection), placementDirection, null);
            }
        }
    }
}
