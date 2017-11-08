using System;
using System.Xml.Serialization;

namespace TheLegendOfDrizzt.Assets.Scripts.Data.Trigger {
    [Serializable]
    public class TextTriggerData : BaseTriggerData {
        [XmlAttribute]
        public string Text { get; set; }
    }
}
