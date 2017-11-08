using System;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using TheLegendOfDrizzt.Assets.Scripts.Model.Trigger;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public class AdventureController : MonoBehaviour {
        private Adventure _adventure;
        private Map _adventureMap;
        private UIController _uiController;

        public void SetUpAdventureController(Adventure adventure, Map adventureMap) {
            _adventure = adventure;
            _adventureMap = adventureMap;
        }

        public void TriggerEvent(string tileTrigger) {
            if (_adventure == null) {
                throw new NullReferenceException($"No Adventure found. Probably forgot to {nameof(SetUpAdventureController)}");
            }

            if (_adventure.Triggers.ContainsKey(tileTrigger)) {
                TriggerEvent(_adventure.Triggers[tileTrigger]);
            }
        }

        private void Awake() {
            _uiController = FindObjectOfType<UIController>();
            if (_uiController == null) { throw new NullReferenceException("No UIController found in scene"); }
        }

        // Use this for initialization
        private void Start() { }

        // Update is called once per frame
        private void Update() { }

        private void TriggerEvent(BaseTrigger trigger) {
            if (trigger is TextTrigger) {
                TriggerTextEvent((TextTrigger)trigger);
            }
            if (trigger is PlaceDoubleTileTrigger) {
                TriggerPlaceDoubleTileEvent((PlaceDoubleTileTrigger)trigger);
            }
        }

        private void TriggerTextEvent(TextTrigger trigger) {
            _uiController.ShowPopupDialog(trigger.Text);
        }

        private void TriggerPlaceDoubleTileEvent(PlaceDoubleTileTrigger trigger) {
            if (_adventureMap == null) {
                throw new NullReferenceException($"No Map found. Probably forgot to {nameof(SetUpAdventureController)}");
            }
            _adventureMap.PlaceDoubleTileToTileWithName(trigger.DoubleTileName, trigger.TileToAttach);
        }
    }
}
