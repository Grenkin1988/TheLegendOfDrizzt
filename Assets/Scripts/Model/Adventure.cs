using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Data;
using TheLegendOfDrizzt.Model.Condition;
using TheLegendOfDrizzt.Model.Trigger;

namespace TheLegendOfDrizzt.Model {
    public class Adventure {
        public string Name { get; }
        public Dictionary<string, BaseTrigger> Triggers { get; }
        public WinningConditionBase WinningCondition { get; }

        public Adventure(AdventureData data) {
            Name = data.Name;
            Triggers = data.Triggers.ToDictionary(trigger => trigger.Name, BaseTrigger.CreateTrigger);
            WinningCondition = WinningConditionBase.CreateTrigger(data.WinningCondition);
        }
    }
}
