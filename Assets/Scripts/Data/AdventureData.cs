using System;
using System.Xml.Serialization;
using TheLegendOfDrizzt.Assets.Scripts.Data.Trigger;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    [Serializable]
    public class AdventureData {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray]
        public BaseTriggerData[] Triggers { get; set; }
    }
}
