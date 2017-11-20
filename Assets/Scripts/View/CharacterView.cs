using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.View {
    public class CharacterView {
        private const string TRAVEL_PATH = "travel_path_";
        private Character _character;

        private GameObject _characterPathGameObject;
        private GameObject _characterGameObject;
        private CustomCharacterController _characterController;

        public CharacterView(Character character) {
            _character = character;
            InitializeCharacter();
        }

        public void Show() {
            if (!_characterGameObject.activeSelf) {
                _characterGameObject.SetActive(true);
            }
        }

        public void Hide() {
            if (_characterGameObject.activeSelf) {
                _characterGameObject.SetActive(false);
            }
        }

        public void DrawPath() {
            Coordinates? coordinates = _character.MovementTargetTile.FindSquareCoordinates(_character.MovementTargetSquare);
            if (coordinates == null) { return; }
            float x = coordinates.Value.X + _character.MovementTargetTile.X + 0.5f;
            float y = coordinates.Value.Y + _character.MovementTargetTile.Y + 0.5f;

            _characterPathGameObject = new GameObject("Path");
            _characterPathGameObject.transform.SetParent(_characterGameObject.transform);
            DrawSteps();
            DrawTarget(x, y);
        }

        public void DestroyPath() {
            Object.Destroy(_characterPathGameObject);
        }

        public void MoveTo(bool isInitialPlace = false) {
            if (isInitialPlace) {
                _characterGameObject.transform.position = new Vector3(_character.X + 0.5f, _character.Y + 0.5f, 0);
            } else {
                _characterController.MoveToBySteps(_character.PathToTarget);
            }
        }

        private void InitializeCharacter() {
            _characterGameObject = new GameObject(_character.Name);
            _characterGameObject.transform.SetParent(GameObject.Find("Players").transform);
            _characterController = _characterGameObject.AddComponent<CustomCharacterController>();
            var characterRenderer = _characterGameObject.AddComponent<SpriteRenderer>();
            characterRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(_character.Name);
            characterRenderer.sortingLayerName = "Player";
            Hide();
        }

        private void DrawSteps() {
            Square previous = _character.PathToTarget.First();
            for (int i = 1; i < _character.PathToTarget.Length; i++) {
                Square current = _character.PathToTarget[i];
                DrawToPathPart(current, previous);
                DrawFromPathPart(current, previous);
                previous = current;
            }
        }

        private void DrawToPathPart(Square current, Square previous) {
            int deltaX = current.MapCoordinates.X - previous.MapCoordinates.X;
            int deltaY = current.MapCoordinates.Y - previous.MapCoordinates.Y;
            string tileName = $"{TRAVEL_PATH}{GetToDirection(deltaX, deltaY)}";

            Coordinates? coordinates = previous.ParentTile.FindSquareCoordinates(previous);
            if (coordinates == null) { return; }
            float targetX = coordinates.Value.X + previous.ParentTile.X + 0.5f;
            float targetY = coordinates.Value.Y + previous.ParentTile.Y + 0.5f;

            var pathGameObject = new GameObject($"Path_To_{current.MapCoordinates.X}_{current.MapCoordinates.Y}");
            pathGameObject.transform.position = new Vector3(targetX, targetY, 0);
            pathGameObject.transform.SetParent(_characterPathGameObject.transform);
            var pathRenderer = pathGameObject.AddComponent<SpriteRenderer>();
            pathRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileName);
            pathRenderer.sortingLayerName = "Path";
        }

        private void DrawFromPathPart(Square current, Square previous) {
            int deltaX = current.MapCoordinates.X - previous.MapCoordinates.X;
            int deltaY = current.MapCoordinates.Y - previous.MapCoordinates.Y;
            string tileName = $"{TRAVEL_PATH}{GetFromDirection(deltaX, deltaY)}";

            Coordinates? coordinates = current.ParentTile.FindSquareCoordinates(current);
            if (coordinates == null) { return; }
            float targetX = coordinates.Value.X + current.ParentTile.X + 0.5f;
            float targetY = coordinates.Value.Y + current.ParentTile.Y + 0.5f;

            var pathGameObject = new GameObject($"Path_From_{previous.MapCoordinates.X}_{previous.MapCoordinates.Y}");
            pathGameObject.transform.position = new Vector3(targetX, targetY, 0);
            pathGameObject.transform.SetParent(_characterPathGameObject.transform);
            var pathRenderer = pathGameObject.AddComponent<SpriteRenderer>();
            pathRenderer.sprite = SpriteManager.Instance.LoadSpriteByName(tileName);
            pathRenderer.sortingLayerName = "Path";
        }

        private string GetToDirection(int deltaX, int deltaY) {
            string end = "to_";
            if (deltaY > 0) {
                end += "N";
            } else if (deltaY != 0) {
                end += "S";
            }

            if (deltaX > 0) {
                end += "E";
            } else if (deltaX != 0) {
                end += "W";
            }

            return end;
        }

        private string GetFromDirection(int deltaX, int deltaY) {
            string end = "from_";
            if (deltaY > 0) {
                end += "S";
            } else if (deltaY != 0) {
                end += "N";
            }

            if (deltaX > 0) {
                end += "W";
            } else if (deltaX != 0) {
                end += "E";
            }

            return end;
        }

        private void DrawTarget(float targetX, float targetY) {
            var targetGameObject = new GameObject($"Target_{_character.MovementTargetSquare.MapCoordinates.X}_{_character.MovementTargetSquare.MapCoordinates.Y}");
            targetGameObject.transform.position = new Vector3(targetX, targetY, 0);
            targetGameObject.transform.SetParent(_characterPathGameObject.transform);
            var targetRenderer = targetGameObject.AddComponent<SpriteRenderer>();
            targetRenderer.sprite = SpriteManager.Instance.LoadSpriteByName($"{TRAVEL_PATH}target");
            targetRenderer.sortingLayerName = "PathTarget";
        }
    }
}
