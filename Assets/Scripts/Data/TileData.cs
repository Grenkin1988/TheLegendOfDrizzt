﻿using System;
using Model;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    [Serializable]
    public class TileData {
        public string Name;
        public string Layout;
        public ArrowColor ArrorColor;
        public SpecialEffect SpecialEffect;

        public override string ToString() {
            return $"{Name}| {ArrorColor}-->>, Effect: {SpecialEffect}";
        }
    }
}
