using System;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    [Serializable]
    public class TileData {
        public string Name;
        public string Layout;
        public string ArrorColor;
        public string SpecialEffect;

        public override string ToString() {
            return $"{Name}| {ArrorColor}-->>, Effect: {SpecialEffect}";
        }
    }
}
