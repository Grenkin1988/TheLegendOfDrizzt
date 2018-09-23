using TheLegendOfDrizzt.Data;

namespace TheLegendOfDrizzt.Model {
    public class Player {
        public string Name { get; }
        public Character Character { get; }

        public Player(PlayerData data, Character character) {
            Name = data.Name;
            Character = character;
        }
    }
}
