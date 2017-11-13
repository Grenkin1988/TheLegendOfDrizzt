﻿namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public struct Coordinates {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}
