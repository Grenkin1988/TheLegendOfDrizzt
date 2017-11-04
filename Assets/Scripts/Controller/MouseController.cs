using System;
using UnityEngine;

namespace Controller {
    public class MouseController : MonoBehaviour {
        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                Vector3 mouseCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseCoordinate, Vector2.zero);
                if (hit.collider != null) {
                    if (hit.collider.gameObject.name.StartsWith("Square_")) {
                        Vector3 position = hit.collider.gameObject.transform.localPosition;
                        int x = (int)position.x;
                        int y = (int)position.y;
                        GameObject tileGameObject = hit.collider.gameObject.transform.parent.gameObject;
                        if (tileGameObject.name.StartsWith("Tile_") && x >= 0 && y >= 0) {
                            OnTileClicked(tileGameObject, x, y);
                        }
                    }
                }
            }
        }

        public event Action<GameObject, int, int> TileClicked;

        protected virtual void OnTileClicked(GameObject tileGameObject, int squareX, int squareY) {
            TileClicked?.Invoke(tileGameObject, squareX, squareY);
        }
    }
}
