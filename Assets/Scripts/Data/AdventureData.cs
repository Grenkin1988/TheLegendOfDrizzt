using System;
using System.Xml.Serialization;
using TheLegendOfDrizzt.Data.Condition;
using TheLegendOfDrizzt.Data.Trigger;

namespace TheLegendOfDrizzt.Data {
    [Serializable]
    [XmlInclude(typeof(TextTriggerData))]
    [XmlInclude(typeof(PlaceDoubleTileTriggerData))]
    [XmlInclude(typeof(StandNearSquareConditionData))]
    [XmlRoot("AdventureData", Namespace = null, IsNullable = false)]
    public class AdventureData {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray]
        public BaseTriggerData[] Triggers { get; set; }

        public WinningConditionBaseData WinningCondition { get; set; }
    }
}
