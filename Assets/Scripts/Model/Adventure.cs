using System.Collections.Generic;
using System.Linq;
using TheLegendOfDrizzt.Assets.Scripts.Data;
using TheLegendOfDrizzt.Assets.Scripts.Model.Trigger;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Adventure {
        public string Name { get; }
        public Dictionary<string, BaseTrigger> Triggers { get; }

        public Adventure(AdventureData data) {
            Name = data.Name;
            Triggers = data.Triggers.ToDictionary(trigger => trigger.Name, BaseTrigger.CreateTrigger);
        }
    }
}
