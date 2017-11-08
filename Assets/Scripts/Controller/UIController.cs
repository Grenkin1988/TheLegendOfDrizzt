﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public class UIController : MonoBehaviour {
        private TurnController _turnController;
        private GameObject _mainCanvas;

        private Text _currecntPlayerText;
        private Text _currentPhaseText;

        private Button _nextPhaseButton;
        private Text _nextPhaseButtonText;

        private Button _moveButton;
        private Button _attackButton;

        public void UpdateUI() {
            UpdateButtonsState();
            UpdateCurrentPlayer();
            UpdateCurrentPhase();
            UpdateNextPhaseButton();
        }

        private void Awake() {
            _mainCanvas = GameObject.Find("MainCanvas");
            if (_mainCanvas == null) { throw new NullReferenceException("No MainCanvas found in scene"); }

            _turnController = FindObjectOfType<TurnController>();
            if (_turnController == null) { throw new NullReferenceException("No TurnController found in scene"); }

            SetUpButtons();
        }

        private void Start() { }

        private void Update() { }

        private void SetUpButtons() {
            _nextPhaseButton = _mainCanvas.transform.Find("NextPhaseButton").GetComponent<Button>();
            _nextPhaseButton.onClick.AddListener(OnNextPhaseButtonClicked);

            _moveButton = _mainCanvas.transform.Find("MoveButton").GetComponent<Button>();
            _moveButton.onClick.AddListener(OnMoveButtonClicked);

            _attackButton = _mainCanvas.transform.Find("AttackButton").GetComponent<Button>();
            _attackButton.onClick.AddListener(OnAttackButtonClicked);
        }

        private void UpdateCurrentPlayer() {            
            if (_currecntPlayerText == null) {
                _currecntPlayerText = _mainCanvas.transform.Find("CurrentPlayerText").GetComponent<Text>();
            }
            _currecntPlayerText.text = $"{_turnController.CurrentPlayer.Name} Turn";
        }

        private void UpdateCurrentPhase() {
            if (_currentPhaseText == null) {
                _currentPhaseText = _mainCanvas.transform.Find("CurrentPhaseText").GetComponent<Text>();
            }
            _currentPhaseText.text = $"{_turnController.CurrentPhase} Phase";
        }

        private void UpdateNextPhaseButton() {
            if (_nextPhaseButtonText == null) {
                _nextPhaseButtonText = _mainCanvas.transform.Find("NextPhaseButton").GetComponentInChildren<Text>();
            }
            string buttonText = "Next ";
            buttonText += _turnController.CurrentPhase == TurnController.Phases.Villain ? "Turn" : "Phase";
            _nextPhaseButtonText.text = buttonText;
        }

        private void UpdateButtonsState() {
            bool heroPhaseButtonsEnabled = _turnController.CurrentPhase == TurnController.Phases.Hero;
            _moveButton.interactable = heroPhaseButtonsEnabled;
            _attackButton.interactable = heroPhaseButtonsEnabled;
        }

        public event Action NextPhaseButtonClicked;
        protected virtual void OnNextPhaseButtonClicked() {
            NextPhaseButtonClicked?.Invoke();
        }

        public event Action MoveButtonClicked;
        protected virtual void OnMoveButtonClicked() {
            MoveButtonClicked?.Invoke();
        }

        public event Action AttackButtonClicked;
        protected virtual void OnAttackButtonClicked() {
            AttackButtonClicked?.Invoke();
        }
    }
}
