using System.Linq;
using UnityEngine;

namespace TheLegendOfDrizzt.Utility {
    public static class GameObjectExtensions {
        public static GameObject FindObject(this GameObject parent, string name) {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            return trs.Where(t => t.name == name).Select(t => t.gameObject).FirstOrDefault();
        }
    }
}
