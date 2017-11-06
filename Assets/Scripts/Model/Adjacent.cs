using System.Collections.Generic;
using System.Linq;

namespace TheLegendOfDrizzt.Assets.Scripts.Model {
    public class Adjacent<T> : Dictionary<Directions, T> where T : class {
        public Adjacent() {
            Initialize();
        }

        public void SetAdjacent(Directions direction, T @object) {
            this[direction] = @object;
        }

        public IList<T> GetAdjacent(params Directions[] direction) {
            IList<T> adjacent = direction.Where(value => this[value] != null).Select(value => this[value]).ToList();
            return adjacent;
        }

        public IList<T> GetAllAdjacent() {
            return GetAdjacent(Enums.GetValues<Directions>().ToArray());
        }

        private void Initialize() {
            foreach (Directions direction in Enums.GetValues<Directions>()) {
                this[direction] = null;
            }
        }
    }
}
