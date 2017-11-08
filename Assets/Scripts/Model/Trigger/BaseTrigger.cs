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
            return new BaseTrigger(data);
        }
    }
}
