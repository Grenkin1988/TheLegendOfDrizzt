using System;
using System.Xml.Serialization;

namespace TheLegendOfDrizzt.Data.Trigger {
    [Serializable]
    public class PlaceDoubleTileTriggerData : BaseTriggerData {
        [XmlAttribute]
        public string DoubleTileName { get; set; }
        [XmlAttribute]
        public string TileToAttach { get; set; }
    }
}
