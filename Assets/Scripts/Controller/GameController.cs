using System;
using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Model;
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

        public void NextPhase() {
            if (!_turnController.NextPhase()) {
                _turnController.TakeTurn(_players[NextplayerIndex()]);
            }
            _uiController.UpdateUI();
        }

        private void Awake() { }

        private void Start() {
            _adventureMap = new Map();
            _mapView = new MapView(_adventureMap);

            SetUpPlayers(new PlayerData[] {
                new PlayerData {
                    Name = "Palayer 1",
                    CharacterData = new CharacterData {
                        Name = "Drizzt"
                    }
                }
            });
            _adventureMap.SetStartingPlayersPosition(_players.ToArray());

            _tileStack = new TileStack();
            PrepareTileStackForAdventure(_tileStack, AdventureController.CurrentAdventureName);

            _mouseController = FindObjectOfType<MouseController>();
            if (_mouseController == null) { throw new NullReferenceException("No MouseController found in scene"); }
            _mouseController.TileClicked += MouseControllerOnTileClicked;

            _turnController = FindObjectOfType<TurnController>();
            if (_turnController == null) { throw new NullReferenceException("No TurnController found in scene"); }
            _turnController.TakeTurn(_players[NextplayerIndex()]);

            _uiController = FindObjectOfType<UIController>();
            if (_uiController == null) { throw new NullReferenceException("No UIController found in scene"); }
            _uiController.NextPhaseButtonClicked += NextPhase;
            _uiController.UpdateUI();
        }

        private void Update() { }

        private void FixedUpdate() { }

        private void OnEnable() { }

        private void OnDisable() { }

        private void OnDestroy() {
            if (_mouseController != null) {
                _mouseController.TileClicked -= MouseControllerOnTileClicked;
            }
            if (_uiController != null) {
                _uiController.NextPhaseButtonClicked -= NextPhase;
            }
            _mapView?.Dispose();
        }

        private void OnApplicationQuit() {
            OnDestroy();
        }

        private void PrepareTileStackForAdventure(TileStack tileStack, string adventureName) {
            tileStack.SetSpecialTile("UndergroundRiver", 8);
            tileStack.GenerateTileStack();
            tileStack.ShuffleTileStack();
        }

        private void MouseControllerOnTileClicked(GameObject tileGameObject, int x, int y) {
            Tile tile = _mapView.GetTileByGameObject(tileGameObject);
            if (tile == null) { return; }
            Directions? placementDirection;
            if (_adventureMap.IsValidPositionForNewTilePlacement(tile, x, y, out placementDirection)) {
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
    }
}
