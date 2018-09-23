using TheLegendOfDrizzt.Data.Trigger;

namespace TheLegendOfDrizzt.Model.Trigger {
    public class TextTrigger : BaseTrigger {
        public string Text { get; }
        
        public TextTrigger(TextTriggerData data)
            : base(data) {
            Text = data.Text;
        }
    }
}
