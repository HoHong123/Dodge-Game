using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Core.Scene;
using TMPro;

namespace Dodge.Game.UI { 
    public class GameOverUI : MonoBehaviour {
        [Title("Panel")]
        [SerializeField]
        GameObject panel;
        [SerializeField]
        GameObject losePanel;
        [SerializeField]
        GameObject winPanel;

        [Title("UI")]
        [SerializeField]
        TMP_Text finalScoreTxt;


        private void Start() {
            GameManager.Instance.OnGameOver += _OnGameOver;
        }


        public void OnClickRestart() {
            SceneLoadManager.Instance.LoadSceneAsync(SceneList.Game);
        }


        private void _OnGameOver(bool result) {
            finalScoreTxt.text = $"Score : {GameManager.Instance.Score.ToString()}";
            panel.SetActive(true);
            if (result) {
                winPanel.SetActive(true);
            }
            else {
                losePanel.SetActive(true);
            }
        }
    }
}