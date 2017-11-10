using System;
using System.Xml.Serialization;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.Condition {
    [Serializable]
    public class StandNearSquareConditionData : WinningConditionBaseData {
        [XmlAttribute]
        public string RelatedTileName { get; set; }
        [XmlAttribute]
        public TerrainTypes Type { get; set; }
        [XmlAttribute]
        public int Distanse { get; set; }
    }
}
