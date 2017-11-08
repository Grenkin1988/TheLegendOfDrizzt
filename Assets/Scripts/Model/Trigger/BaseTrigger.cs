using TheLegendOfDrizzt.Assets.Scripts.Data.Trigger;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.Trigger {
    public class BaseTrigger {
        public string Name { get; }

        public BaseTrigger(BaseTriggerData data) {
            Name = data.Name;
        }

        public static BaseTrigger CreateTrigger(BaseTriggerData data) {
            if (data is TextTriggerData) {
                return new TextTrigger((TextTriggerData)data);
            }
            if (data is PlaceDoubleTileTriggerData) {
                return new PlaceDoubleTileTrigger((PlaceDoubleTileTriggerData)data);
            }
            return new BaseTrigger(data);
        }
    }
}
