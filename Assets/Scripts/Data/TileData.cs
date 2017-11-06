using System;
using System.Xml.Serialization;
using TheLegendOfDrizzt.Assets.Scripts.Model;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    [Serializable]
    public class TileData {
        [XmlAttribute]
        public string Name { get; set; }
        public string Layout { get; set; }
        [XmlAttribute]
        public ArrowColor ArrowColor { get; set; }
        [XmlAttribute]
        public SpecialEffect SpecialEffect { get; set; }

        public override string ToString() {
            return $"{Name}| {ArrowColor}-->>, Effect: {SpecialEffect}";
        }
    }
}
