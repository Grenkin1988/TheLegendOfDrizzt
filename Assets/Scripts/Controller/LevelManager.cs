using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public static class LevelManager {
        private const string START_MENU_SCENE = "Start Menu";
        private const string ADVENTURE_SCENE = "Adventure";

        public static void LoadAdventure(string name) {
            Debug.Log($"Load adventure {name}");
            AdventureManager.CurrentAdventureName = name;
            SceneManager.LoadScene(ADVENTURE_SCENE);
        }

        public static void QuitRequest() {
            Debug.Log("Quit requsted");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static void GoToMainMenu() {
            SceneManager.LoadScene(START_MENU_SCENE);
        }
    }
}
