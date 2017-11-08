using TheLegendOfDrizzt.Assets.Scripts.Data.Trigger;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.Trigger {
    public class TextTrigger : BaseTrigger {
        public string Text { get; }
        
        public TextTrigger(TextTriggerData data)
            : base(data) {
            Text = data.Text;
        }
    }
}
