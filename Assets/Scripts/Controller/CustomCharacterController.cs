using System.Collections;
using JetBrains.Annotations;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    [UsedImplicitly]
    public class CustomCharacterController : MonoBehaviour {

        [SerializeField]
        private float _moveSpeed = 0.5f;
        private bool _isMoving = false;
        private Vector3 _nextTarget;

        [UsedImplicitly]
        private void Start() {

        }

        [UsedImplicitly]
        private void Update() {
        }

        public void MoveToBySteps(Square[] characterPathToTarget) {
            for (int i = 1; i < characterPathToTarget.Length; i++) {
                _isMoving = true;
                Square next = characterPathToTarget[i];
                Coordinates? nextCoordinates = next.ParentTile.FindSquareCoordinates(next);
                if (!nextCoordinates.HasValue) { continue; }
                float targetX = nextCoordinates.Value.X + next.ParentTile.X + 0.5f;
                float targetY = nextCoordinates.Value.Y + next.ParentTile.Y + 0.5f;
                _nextTarget = new Vector3(targetX, targetY, 0);
                while (_isMoving) {
                    Move();
                }
            }
        }

        private void Move() {
            float step = _moveSpeed;
            transform.position = Vector3.Lerp(transform.position, _nextTarget, step);
            if ((transform.position - _nextTarget).magnitude <= 0.05f) {
                _isMoving = false;
            }
        }
    }
}
