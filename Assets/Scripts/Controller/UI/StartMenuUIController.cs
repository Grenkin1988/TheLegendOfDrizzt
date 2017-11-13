using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller.UI {
    public class StartMenuUIController : MonoBehaviour {
        private GameObject _mainCanvas;

        private Button _selectAdventureButton;
        private Button _exitButton;

        private void Awake() {
            _mainCanvas = GameObject.Find("MainCanvas");
            if (_mainCanvas == null) { throw new NullReferenceException("No MainCanvas found in scene"); }

            SetUpButtons();
        }

        private void Start() { }

        private void Update() { }

        private void SetUpButtons() {
            _selectAdventureButton = _mainCanvas.transform.Find("SelectAdventureButton").GetComponent<Button>();
            _selectAdventureButton.onClick.AddListener(() => LevelManager.LoadAdventure("Adventure1"));

            _exitButton = _mainCanvas.transform.Find("ExitButton").GetComponent<Button>();
            _exitButton.onClick.AddListener(LevelManager.QuitRequest);
        }
    }
}
