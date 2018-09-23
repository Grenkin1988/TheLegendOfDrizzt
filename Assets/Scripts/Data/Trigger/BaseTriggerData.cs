using System;
using System.Xml.Serialization;

namespace TheLegendOfDrizzt.Data.Trigger {
    [Serializable]
    public class BaseTriggerData {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
