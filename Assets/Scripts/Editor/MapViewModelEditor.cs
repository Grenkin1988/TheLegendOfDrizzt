using TheLegendOfDrizzt.Assets.Scripts.View;
using UnityEditor;

namespace TheLegendOfDrizzt.Assets.Scripts.Editor {
    [CustomEditor(typeof(MapView))]
    public class MapViewModelEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}
