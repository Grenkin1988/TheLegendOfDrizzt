using System;
using System.Collections.Generic;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using TheLegendOfDrizzt.Assets.Scripts.Utility;
using TheLegendOfDrizzt.Assets.Scripts.View;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public class GameController : MonoBehaviour {
        private Map _adventureMap;
        private MapView _mapView;
        private TileStack _tileStack;
        private MouseController _mouseController;
        private TurnController _turnController;
        private UIController _uiController;
        private List<Player> _players = new List<Player>();
        private int? _nextPlayerIndex = null;
        private AdventureController _adventureController;

        private void Awake() {
            _mouseController = FindObjectOfType<MouseController>();
            if (_mouseController == null) { throw new NullReferenceException("No MouseController found in scene"); }
            _mouseController.TileClicked += MouseControllerOnTileClicked;

            _turnController = FindObjectOfType<TurnController>();
            if (_turnController == null) { throw new NullReferenceException("No TurnController found in scene"); }

            _adventureController = FindObjectOfType<AdventureController>();
            if (_adventureController == null) { throw new NullReferenceException("No AdventureController found in scene"); }

            _uiController = FindObjectOfType<UIController>();
            if (_uiController == null) { throw new NullReferenceException("No UIController found in scene"); }
            _uiController.NextPhaseButtonClicked += NextPhase;
            _uiController.MoveButtonClicked += SetMoveMode;
            _uiController.AttackButtonClicked += SetAttackMode;

            _uiController.Debug_PlaceTileButtonClicked += SetDebug_PlaceTileMode;
        }

        private void Start() {
            _adventureMap = new Map();
            _mapView = new MapView(_adventureMap);
            _adventureMap.NewTileCreated += AdventureMapOnNewTileCreated;

            SetUpPlayers(AdventureManager.GetDefaultPlayers());
            _adventureController.SetUpAdventureController(new Adventure(AdventureManager.GetDefaultAdventure1()), _adventureMap);
            _adventureMap.SetStartingPlayersPosition(_players.ToArray());

            _tileStack = new TileStack();
            PrepareTileStackForAdventure(_tileStack, AdventureManager.CurrentAdventureName);

            _turnController.TakeTurn(_players[NextplayerIndex()]);

            _uiController.UpdateUI();
            TriggerEvent("StartTile");
        }

        private void Update() { }

        private void FixedUpdate() { }

        private void OnEnable() { }

        private void OnDisable() { }

        private void OnDestroy() {
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
                _uiController.Debug_PlaceTileButtonClicked -= SetDebug_PlaceTileMode;
            }
            _mapView?.Dispose();
        }

        private void OnApplicationQuit() {
            OnDestroy();
        }

        private void AdventureMapOnNewTileCreated(Tile tile) {
            if (!string.IsNullOrEmpty(tile.Trigger)) {
                TriggerEvent(tile.Trigger);
            }
        }

        private void TriggerEvent(string tileTrigger) {
            _adventureController.TriggerEvent(tileTrigger);
        }

        private void PrepareTileStackForAdventure(TileStack tileStack, string adventureName) {
            tileStack.SetSpecialTile("UndergroundRiver", 8);
            tileStack.GenerateTileStack();
            tileStack.ShuffleTileStack();
        }

        private void MouseControllerOnTileClicked(GameObject tileGameObject, int x, int y) {
            Tile tile = _mapView.GetTileByGameObject(tileGameObject);
            if (tile == null) { return; }

            if (_mouseController.CurrentMode == MouseController.MouseModes.Move 
                && tile.CanMoveHere(x, y, _turnController.CurrentPlayer.Character)) {
                _turnController.CurrentPlayer.Character.MoveHere(x, y, tile);
                _mouseController.ChangeMouseMode(MouseController.MouseModes.None);
            }

            Directions? placementDirection;
            if (_mouseController.CurrentMode == MouseController.MouseModes.Debug_PlaceTile 
                && _adventureMap.IsValidPositionForNewTilePlacement(tile, x, y, out placementDirection)) {
                if (!placementDirection.HasValue) { return; }
                Tile newTile = _tileStack.GetNexTile();
                if (newTile != null) {
                    _adventureMap.PlaceNewTileNearExistent(tile, newTile, placementDirection.Value);
                }
            }
        }

        private void SetUpPlayers(params PlayerData[] data) {
            foreach (PlayerData playerData in data) {
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
            _mouseController.ChangeMouseMode(MouseController.MouseModes.None);

            ExecutePhaseEnd();

            if (!_turnController.NextPhase()) {
                _turnController.TakeTurn(_players[NextplayerIndex()]);
            }
            _uiController.UpdateUI();
            ExecutePhase();
        }

        private void SetMoveMode() {
            _mouseController.ChangeMouseMode(MouseController.MouseModes.Move);
        }

        private void SetAttackMode() {
            _mouseController.ChangeMouseMode(MouseController.MouseModes.Attack);
        }

        private void SetDebug_PlaceTileMode() {
            _mouseController.ChangeMouseMode(MouseController.MouseModes.Debug_PlaceTile);
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
            Directions? placementDirection;
            Character character = _turnController.CurrentPlayer.Character;
            if (_adventureMap.IsValidPositionForNewTilePlacement(
                character.CurrentTile, 
                character.X - character.CurrentTile.X,
                character.Y - character.CurrentTile.Y, 
                out placementDirection)) {
                if (!placementDirection.HasValue) { return; }
                Tile newTile = _tileStack.GetNexTile();
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
