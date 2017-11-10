
using System;

namespace TheLegendOfDrizzt.Assets.Scripts.Model.Condition {
    public abstract class WinningConditionBase {
        public abstract bool IsConditionMet(Player palyer);

        public static WinningConditionBase CreateTrigger(IWinningConditionData data) {
            if (data is StandNearSquareConditionData) {
                return new StandNearSquareCondition((StandNearSquareConditionData)data);
            }
            throw new NotImplementedException(data.ToString());
        }
    }
}
