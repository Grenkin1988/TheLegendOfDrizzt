using UnityEngine;

namespace Controller {
    public class CameraController : MonoBehaviour {
        private Vector3 LastFrameCameraPosition;

        public float ZoomFactor = 2.5f;
        public float MinZoom = 4.0f;
        public float MaxZoom = 15.0f;

        private void Update() {
            Vector3 currentFrameCameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
                Vector3 difference = LastFrameCameraPosition - currentFrameCameraPosition;
                Camera.main.transform.Translate(difference);
            }

            float sw = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(sw) > 0.01f) {
                Camera.main.orthographicSize += sw * ZoomFactor;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MinZoom, MaxZoom);
            }
            LastFrameCameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
