using System;
using System.Xml.Serialization;
using TheLegendOfDrizzt.Assets.Scripts.Model;

namespace TheLegendOfDrizzt.Assets.Scripts.Data.Condition {
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
