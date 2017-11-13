using TheLegendOfDrizzt.Assets.Scripts.Controller;
using UnityEditor;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Editor {
    [CustomEditor(typeof(MouseController))]
    public class MouseControllerEditor : UnityEditor.Editor {
        private MouseController _mouseController;
        private MouseController.MouseModes _currentMode;


        public override void OnInspectorGUI() {
            _mouseController = (MouseController)target;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Current mode", _mouseController.CurrentMode.ToString());

            _currentMode = (MouseController.MouseModes)EditorGUILayout.EnumPopup("Mouse mode", _currentMode);
            if(GUILayout.Button("Set mouse mode")) {
                _mouseController.ChangeMouseMode(_currentMode);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
