namespace TheLegendOfDrizzt.Model {
    public struct Coordinates {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsDiagonal { get; set; }

        public Coordinates(int x, int y, bool isDiagonal = false) {
            X = x;
            Y = y;
            IsDiagonal = isDiagonal;
        }

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}
