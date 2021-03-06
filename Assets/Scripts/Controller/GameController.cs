﻿using System;
using System.Collections.Generic;
using TheLegendOfDrizzt.Controller.UI;
using TheLegendOfDrizzt.Data;
using TheLegendOfDrizzt.Model;
using TheLegendOfDrizzt.View;
using UnityEngine;

namespace TheLegendOfDrizzt.Controller {
    public class GameController : MonoBehaviour {
        private Map _adventureMap;
        private TileStack _tileStack;
        private List<Player> _players = new List<Player>();
        private int? _nextPlayerIndex;

        private MapView _mapView;

        private MouseController _mouseController;
        private TurnController _turnController;
        private AdventureUIController _uiController;
        private AdventureController _adventureController;

        private void Awake() {
            _mouseController = FindObjectOfType<MouseController>();
            if (_mouseController == null) { throw new NullReferenceException("No MouseController found in scene"); }

            _turnController = FindObjectOfType<TurnController>();
            if (_turnController == null) { throw new NullReferenceException("No TurnController found in scene"); }

            _uiController = FindObjectOfType<AdventureUIController>();
            if (_uiController == null) { throw new NullReferenceException("No UIController found in scene"); }
        }

        private void Start() {
            _adventureMap = new Map();
            _mapView = new MapView(_adventureMap);

            SetUpPlayers(AdventureManager.GetDefaultPlayers());
            _adventureController = new AdventureController(new Adventure(AdventureManager.GetDefaultAdventure1()), _adventureMap, _uiController);
            _adventureMap.SetStartingPlayersPosition(_players.ToArray());

            _tileStack = new TileStack();
            _adventureController.PrepareTileStackForAdventure(_tileStack, AdventureManager.CurrentAdventureName);

            AttachEventHandlers();

            _turnController.TakeTurn(_players[NextplayerIndex()]);

            _uiController.UpdateUI();
            TriggerEvent("StartTile");
        }

        private void OnDestroy() {
            DetachEventHandlers();
            _mapView?.Dispose();
        }

        private void OnApplicationQuit() => OnDestroy();

        private void AttachEventHandlers() {
            if (_adventureMap != null) {
                _adventureMap.NewTileCreated += AdventureMapOnNewTileCreated;
            }

            if (_mouseController != null) {
                _mouseController.TileClicked += MouseControllerOnTileClicked;
            }

            if (_uiController != null) {
                _uiController.NextPhaseButtonClicked += NextPhase;
                _uiController.MoveButtonClicked += SetMoveMode;
                _uiController.AttackButtonClicked += SetAttackMode;
            }
        }

        private void DetachEventHandlers() {
            if (_adventureMap != null) {
                _adventureMap.NewTileCreated -= AdventureMapOnNewTileCreated;
            }

            if (_mouseController != null) {
                _mouseController.TileClicked -= MouseControllerOnTileClicked;
            }

            if (_uiController != null) {
                _uiController.NextPhaseButtonClicked -= NextPhase;
                _uiController.MoveButtonClicked -= SetMoveMode;
                _uiController.AttackButtonClicked -= SetAttackMode;
            }
        }

        private void AdventureMapOnNewTileCreated(Tile tile) {
            if (!string.IsNullOrEmpty(tile.Trigger)) {
                TriggerEvent(tile.Trigger);
            }
        }

        private void TriggerEvent(string tileTrigger) {
            _adventureController.TriggerEvent(tileTrigger);
        }

        private void MouseControllerOnTileClicked(GameObject tileGameObject, int x, int y) {
            var tile = _mapView.GetTileByGameObject(tileGameObject);
            if (tile == null) { return; }

            if (_mouseController.CurrentMode == MouseController.MouseModes.Move
                && tile.CanMoveHere(x, y)) {

                if (_turnController.CurrentPlayer.Character.UpdateMovementTarget(x, y, tile)) {
                    _turnController.CurrentPlayer.Character.UpdatePath();
                    return;
                }

                if (_turnController.CurrentPlayer.Character.MoveToTarget(x, y, tile)) {
                    SetNoneMode();
                }
            }

            if (_mouseController.CurrentMode == MouseController.MouseModes.DEBUG_PLACE_TILE
                && _adventureMap.IsValidPositionForNewTilePlacement(tile, x, y, out var placementDirection)) {
                if (!placementDirection.HasValue) { return; }
                var newTile = _tileStack.GetNexTile();
                if (newTile != null) {
                    _adventureMap.PlaceNewTileNearExistent(tile, newTile, placementDirection.Value);
                }
            }
        }

        private void SetUpPlayers(params PlayerData[] data) {
            foreach (var playerData in data) {
                var character = new Character(playerData.CharacterData);
                var player = new Player(playerData, character);
                _players.Add(player);
            }
        }

        private int NextplayerIndex() {
            if (!_nextPlayerIndex.HasValue) {
                _nextPlayerIndex = 0;
                return _nextPlayerIndex.Value;
            }
            _nextPlayerIndex++;
            if (_nextPlayerIndex >= _players.Count) {
                _nextPlayerIndex = 0;
            }
            return _nextPlayerIndex.Value;
        }

        private void NextPhase() {
            SetNoneMode();

            ExecutePhaseEnd();

            if (!_turnController.NextPhase()) {
                _turnController.TakeTurn(_players[NextplayerIndex()]);
            }
            _uiController.UpdateUI();
            ExecutePhase();
        }

        private void SetNoneMode() {
            ResetPath();
            _mouseController.ChangeMouseMode(MouseController.MouseModes.None);
        }

        private void SetMoveMode() {
            _turnController.CurrentPlayer.Character.RecalculatePathfinding(_adventureMap.SquaresMap);
            _mapView.DrawReachableZone(_turnController.CurrentPlayer.Character);
            _mouseController.ChangeMouseMode(MouseController.MouseModes.Move);
        }

        private void SetAttackMode() {
            ResetPath();
            _mouseController.ChangeMouseMode(MouseController.MouseModes.Attack);
        }

        private void ResetPath() {
            _mapView.ResetReachableZone();
            _turnController.CurrentPlayer.Character.ResetPath();
        }

        private void ExecutePhase() {
            switch (_turnController.CurrentPhase) {
                case TurnController.Phases.Hero: { return; }
                case TurnController.Phases.Exploration: {
                    ExecuteExplorationPhase();
                    return;
                }
                case TurnController.Phases.Villain: {
                    ExecuteVillainPhase();
                    return;
                }
                default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteExplorationPhase() {
            var character = _turnController.CurrentPlayer.Character;
            if (_adventureMap.IsValidPositionForNewTilePlacement(
                character.CurrentTile,
                character.X - character.CurrentTile.X,
                character.Y - character.CurrentTile.Y,
                out var placementDirection)) {
                if (!placementDirection.HasValue) { return; }
                var newTile = _tileStack.GetNexTile();
                if (newTile != null) {
                    _adventureMap.PlaceNewTileNearExistent(character.CurrentTile, newTile, placementDirection.Value);
                }
            }
        }

        private void ExecuteVillainPhase() { }

        private void ExecutePhaseEnd() {
            switch (_turnController.CurrentPhase) {
                case TurnController.Phases.Hero: {
                    ExecuteHeroEnd();
                    return;
                }
                case TurnController.Phases.Exploration: {
                    //ExecuteExplorationPhaseEnd();
                    return;
                }
                case TurnController.Phases.Villain: {
                    //ExecuteVillainPhaseEnd();
                    return;
                }
                default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteHeroEnd() {
            bool doWeWin = _adventureController.CheckWinningCondition(_turnController.CurrentPlayer);
            if (doWeWin) {
                _uiController.ShowWinScreenDialog("WE WIN!!!!!! ARAAAAAAA");
            }
        }
    }
}
