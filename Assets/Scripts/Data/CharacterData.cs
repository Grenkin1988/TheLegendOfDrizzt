using System;
using System.Xml.Serialization;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    [Serializable]
    public class CharacterData {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int ArmorClass { get; set; }
        [XmlAttribute]
        public int HitPoints { get; set; }
        [XmlAttribute]
        public int Speed { get; set; }
        [XmlAttribute]
        public int SurgeValue { get; set; }
        [XmlAttribute]
        public int Size { get; set; }
    }
}
