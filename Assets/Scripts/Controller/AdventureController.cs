using System;
using TheLegendOfDrizzt.Controller.UI;
using TheLegendOfDrizzt.Model;
using TheLegendOfDrizzt.Model.Condition;
using TheLegendOfDrizzt.Model.Trigger;

namespace TheLegendOfDrizzt.Controller {
    public class AdventureController {
        private Adventure _adventure;
        private Map _adventureMap;
        private AdventureUIController _uiController;

        public AdventureController(Adventure adventure, Map adventureMap, AdventureUIController uiController) {
            _adventure = adventure;
            _adventureMap = adventureMap;
            _uiController = uiController;
        }

        public void PrepareTileStackForAdventure(TileStack tileStack, string adventureName) {
            tileStack.SetSpecialTile("UndergroundRiver", 8);
            tileStack.GenerateTileStack();
            tileStack.ShuffleTileStack();
        }

        public void TriggerEvent(string tileTrigger) {
            if (_adventure == null) {
                throw new NullReferenceException(nameof(_adventure));
            }

            if (_adventure.Triggers.ContainsKey(tileTrigger)) {
                TriggerEvent(_adventure.Triggers[tileTrigger]);
            }
        }

        public bool CheckWinningCondition(Player player) {
            if (_adventure.WinningCondition is StandNearSquareCondition) {
                return CheckWinningCondition(player, (StandNearSquareCondition)_adventure.WinningCondition);
            }
            return false;
        }

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
                throw new NullReferenceException(nameof(_adventureMap));
            }
            _adventureMap.PlaceDoubleTileToTileWithName(trigger.DoubleTileName, trigger.TileToAttach);
        }

        private bool CheckWinningCondition(Player player, StandNearSquareCondition winningCondition) {
            winningCondition.SetUpCondition(_adventureMap);
            return winningCondition.IsConditionMet(player);
        }
    }
}
