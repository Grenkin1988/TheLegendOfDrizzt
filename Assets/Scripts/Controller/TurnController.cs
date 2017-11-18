using JetBrains.Annotations;
using TheLegendOfDrizzt.Assets.Scripts.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    [UsedImplicitly]
    public class TurnController : MonoBehaviour {

        public enum Phases {
            Hero = 1,
            Exploration = 2,
            Villain = 3
        }

        public Phases CurrentPhase { get; private set; }
        public Player CurrentPlayer { get; private set; }

        public void TakeTurn(Player player) {
            CurrentPlayer = player;
            CurrentPhase = Phases.Hero;
        }

        public bool NextPhase() {
            if (CurrentPhase == Phases.Villain) { return false; }
            CurrentPhase++;
            return true;
        }

        // Use this for initialization
        [UsedImplicitly]
        private void Start () {
		
        }
	
        // Update is called once per frame
        [UsedImplicitly]
        private void Update () {
		
        }
    }
}
