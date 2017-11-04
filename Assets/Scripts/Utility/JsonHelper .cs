using System;

namespace TheLegendOfDrizzt.Assets.Scripts.Utility {
    public static class JsonHelper {
        public static T[] FromJson<T>(string json) {
            var wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            var wrapper = new Wrapper<T> {
                Items = array
            };
            return UnityEngine.JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T> {
            public T[] Items;
        }
    }
}
