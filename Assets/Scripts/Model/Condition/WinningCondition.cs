using System;
using TheLegendOfDrizzt.Data.Condition;

namespace TheLegendOfDrizzt.Model.Condition {
    public abstract class WinningConditionBase {
        public abstract bool IsConditionMet(Player palyer);

        public static WinningConditionBase CreateTrigger(WinningConditionBaseData data) {
            if (data is StandNearSquareConditionData) {
                return new StandNearSquareCondition((StandNearSquareConditionData)data);
            }
            throw new NotImplementedException(data.ToString());
        }
    }
}
