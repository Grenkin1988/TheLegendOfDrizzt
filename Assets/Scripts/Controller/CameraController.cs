using UnityEngine;

namespace TheLegendOfDrizzt.Controller {
    public class CameraController : MonoBehaviour {
        private Vector3 _lastFrameCameraPosition;

        [SerializeField]
        private float _zoomFactor = 2.5f;
        [SerializeField]
        private float _minZoom = 4.0f;
        [SerializeField]
        private float _maxZoom = 15.0f;

        private void Update() {
            var currentFrameCameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
                var difference = _lastFrameCameraPosition - currentFrameCameraPosition;
                Camera.main.transform.Translate(difference);
            }

            float sw = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(sw) > 0.01f) {
                Camera.main.orthographicSize += sw * _zoomFactor;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
            }
            _lastFrameCameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
