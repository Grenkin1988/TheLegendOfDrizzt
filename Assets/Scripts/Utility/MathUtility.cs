using TheLegendOfDrizzt.Assets.Scripts.Model;

namespace TheLegendOfDrizzt.Assets.Scripts.Utility {
    public static class MathUtility {
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

        public static Coordinates CoordinatesOf<T>(this T[,] matrix, T value) {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x) {
                for (int y = 0; y < h; ++y) {
                    T value2 = matrix[x, y];
                    if (value2 == null) { continue; }
                    if (matrix[x, y].Equals(value)) {
                        return new Coordinates(x, y);
                    }
                }
            }

            return new Coordinates(-1, -1);
        }
    }
}
