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
        Chasm = 5,
        Bridge = 6,
        Pillar = 7,
        River = 8,
        Crystal = 10,
        Campfire = 11,
        DwarfStatue = 12
    }

    public enum Directions {
        South = 1,
        West = 2,
        North = 3,
        East = 4,
    }

    public enum ArrowColor {
        White = 1,
        Black = 2,
    }

    public enum SpecialEffect {
        None = 0,
        VolcanicVent = 1,
        NarrowPassage = 2,
        DarkChasm = 3,
        BrokerDoor = 4,
        UndergroundRiver = 5,
        DrowGlyph = 6,
        CrystalShard = 7,
        SecretCave = 8,
        DwarvenStatue = 9
    }
}
