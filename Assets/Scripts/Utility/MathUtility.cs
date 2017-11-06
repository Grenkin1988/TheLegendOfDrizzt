
namespace TheLegendOfDrizzt.Assets.Scripts.Utility {
    public class MathUtility {
        public static T[,] RotateArrayClockwise<T>(T[,] inputArray, int arraySize) {
            var newArray = new T[arraySize, arraySize];
            for (int i = arraySize - 1; i >= 0; --i) {
                for (int j = 0; j < arraySize; ++j) {
                    newArray[j, arraySize - 1 - i] = inputArray[i, j];
                }
            }
            return newArray;
        }

        public static T[,] RotateArrayCounterClockwise<T>(T[,] inputArray, int arraySize) {
            var newArray = new T[arraySize, arraySize];
            for (int i = 0; i < arraySize; ++i) {
                for (int j = 0; j < arraySize; ++j) {
                    newArray[i, j] = inputArray[j, arraySize - 1 - i];
                }
            }
            return newArray;
        }
    }
}
