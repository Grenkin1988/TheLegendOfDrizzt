using TheLegendOfDrizzt.Data;
using TheLegendOfDrizzt.Data.Condition;
using TheLegendOfDrizzt.Data.Trigger;
using TheLegendOfDrizzt.Model;

namespace TheLegendOfDrizzt.Controller {
    public static class AdventureManager {
        public static string CurrentAdventureName { get; set; } = "Adventure1";

        public static PlayerData[] GetDefaultPlayers() {
            var players = new[] {
                new PlayerData {
                    Name = "Palayer 1",
                    CharacterData = new CharacterData {
                        Name = "Drizzt",
                        ArmorClass = 16,
                        HitPoints = 8,
                        Speed = 3,
                        SurgeValue = 4,
                        Size = 1
                    }
                }
            };
            return players;
        }

        public static AdventureData GetDefaultAdventure1() {
            var adventureData = new AdventureData {
                Name = "Adventure1",
                Triggers = new BaseTriggerData[] {
                    new TextTriggerData {
                        Name = "StartTile",
                        Text = "Life is normally difficult in the Underdark, but the spider goddess Lolth's demand for your sacrifice has made it impossible. You are left with one choice: You must fight your way through the Underdark and find your way to the surface."
                    },
                    new TextTriggerData {
                        Name = "SurfaceHollow",
                        Text = "The sound of running water is a welcome respite from the silence of the caves. After days of traveling through the dark, you see the soft glow of sunlight ahead. Only a few monsters stand between you and freedom!"
                    },
                    new PlaceDoubleTileTriggerData {
                        Name = "UndergroundRiver",
                        DoubleTileName = "SurfaceHollow",
                        TileToAttach = "UndergroundRiver"
                    }
                },
                WinningCondition = new StandNearSquareConditionData {
                    RelatedTileName = "SurfaceHollow_2",
                    Type = TerrainTypes.Exit,
                    Distanse = 1
                }
            };

            return adventureData;
        }
    }
}
