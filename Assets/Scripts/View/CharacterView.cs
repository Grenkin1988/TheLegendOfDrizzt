using TheLegendOfDrizzt.Assets.Scripts.Controller;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.View {
    public class CharacterView {
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
            var targetGameObject = new GameObject($"Target_{_character.MovementTargetSquare.MapCoordinates.X}_{_character.MovementTargetSquare.MapCoordinates.Y}");
            targetGameObject.transform.position = new Vector3(x, y, 0);
            targetGameObject.transform.SetParent(_characterPathGameObject.transform);
            var characterRenderer = targetGameObject.AddComponent<SpriteRenderer>();
            characterRenderer.sprite = SpriteManager.Instance.LoadSpriteByName("travel_path_target");
            characterRenderer.sortingLayerName = "PathTarget";
        }

        public void DestroyPath() {
            Object.Destroy(_characterPathGameObject);
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

        public void MoveTo() {
            _characterGameObject.transform.position = new Vector3(_character.X + 0.5f, _character.Y + 0.5f, 0);
        }
    }
}
