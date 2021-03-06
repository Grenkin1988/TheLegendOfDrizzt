﻿using TheLegendOfDrizzt.Model;
using UnityEngine;

namespace TheLegendOfDrizzt.Controller {
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
    }
}
