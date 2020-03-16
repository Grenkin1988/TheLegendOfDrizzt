using System;

namespace TheLegendOfDrizzt.Utility {
    public static class FisherYatesShuffle {
        private static readonly Random _random = new Random();

        public static void ShuffleSequence<T>(T[] sequence) {
            for (int n = sequence.Length - 1; n > 0; --n) {
                int k = _random.Next(n + 1);
                var temp = sequence[n];
                sequence[n] = sequence[k];
                sequence[k] = temp;
            }
        }
    }
}
