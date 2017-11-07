using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheLegendOfDrizzt.Assets.Scripts.Controller {
    public class LevelManager : MonoBehaviour {
        private const string ADVENTURE_SCENE = "Adventure";

        public void LoadAdventure(string name) {
            Debug.Log($"Load adventure {name}");
            AdventureController.CurrentAdventureName = name;
            SceneManager.LoadScene(ADVENTURE_SCENE);
        }

        public void QuitRequest() {
            Debug.Log("Quit requsted");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
