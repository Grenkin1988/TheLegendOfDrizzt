using System;
using System.Collections.Generic;
using System.Linq;

namespace Model {
    public static class Enums {
        public static IEnumerable<T> GetValues<T>() where T : struct, IConvertible {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("T must be an enumerated type");
            }
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ParceEnumValue<T>(string enumValueText) where T : struct, IConvertible {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("T must be an enumerated type");
            }
            return (T)Enum.Parse(typeof(T), enumValueText);
        }
    }

    public enum TerrainTypes {
        Floor = 1,
        Wall = 2,
        Mashrooms = 3,
        VolcanicVent = 4,
    }

    public enum Directions {       
        South = 1,
        West = 2,
        North = 3,
        East = 4,
    }
}
