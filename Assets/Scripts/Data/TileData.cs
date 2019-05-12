using System;
using System.Xml.Serialization;
using TheLegendOfDrizzt.Model;

namespace TheLegendOfDrizzt.Data {
    [Serializable]
    public class TileData {
        [XmlAttribute]
        public string Name { get; set; }
        public string Layout { get; set; }
        [XmlAttribute]
        public ArrowColor ArrowColor { get; set; }
        [XmlAttribute]
        public SpecialEffect SpecialEffect { get; set; }
        [XmlAttribute]
        public string Trigger { get; set; }
        [XmlAttribute]
        public string Decal { get; set; }

        public override string ToString() {
            return $"{Name}| {ArrowColor}-->>, Effect: {SpecialEffect}";
        }
    }
}
