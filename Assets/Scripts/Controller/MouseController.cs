using System;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public class MouseController : MonoBehaviour {
        public enum MouseModes {
            None = 0,
            Move = 1,
            Attack = 2,

            Debug_PlaceTile = 10001
        }

        public MouseModes CurrentMode { get; private set; }= MouseModes.None;

        public void ChangeMouseMode(MouseModes newMode) {
            CurrentMode = newMode;
        }

        private void Update() {
            if ((CurrentMode == MouseModes.Move || CurrentMode == MouseModes.Debug_PlaceTile) 
                && Input.GetMouseButtonDown(0)) {
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
