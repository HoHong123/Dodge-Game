using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Dodge.Game.UI {
    public class GameProgressUI : MonoBehaviour {
        [Title("Panel")]
        [SerializeField]
        TMP_Text scoreTxt;


        private void Start() {
            GameManager.Instance.OnGameOver += (result) => { _OnScoreChange(); };
            GameManager.Instance.OnScoreChange += (score) => { _OnScoreChange(); };
        }


        private void _OnScoreChange() {
            scoreTxt.text = GameManager.Instance.Score.ToString();
        }
    }
}