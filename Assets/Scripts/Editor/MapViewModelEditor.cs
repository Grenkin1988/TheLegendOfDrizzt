using UnityEditor;
using ViewModel;

namespace Editor {
    [CustomEditor(typeof(MapViewModel))]
    public class MapViewModelEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}
