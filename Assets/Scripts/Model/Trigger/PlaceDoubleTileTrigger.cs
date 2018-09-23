using TheLegendOfDrizzt.Data.Trigger;

namespace TheLegendOfDrizzt.Model.Trigger {
    public class PlaceDoubleTileTrigger : BaseTrigger {
        public string DoubleTileName { get; }
        public string TileToAttach { get; }

        public PlaceDoubleTileTrigger(PlaceDoubleTileTriggerData data) : base(data) {
            DoubleTileName = data.DoubleTileName;
            TileToAttach = data.TileToAttach;
        }
    }
}
