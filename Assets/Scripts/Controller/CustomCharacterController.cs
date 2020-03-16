using System.Threading.Tasks;
using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Controller {
    public class CustomCharacterController : MonoBehaviour {

        [SerializeField]
        private float _moveSpeed = 0.1f;
        private bool _isMoving = false;
        private Vector3 _nextTarget;

        private void Update() {
            while (_isMoving) {
                float step = _moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _nextTarget, step);
                if ((transform.position - _nextTarget).magnitude <= 0.05f) {
                    _isMoving = false;
                }
            }
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
                Update();
                while (_isMoving) {
                    Task.Delay(100);
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
