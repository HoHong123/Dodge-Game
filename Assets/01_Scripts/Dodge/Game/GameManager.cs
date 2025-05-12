using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Dodge.Game.UI;

namespace Dodge.Game {
    public class GameManager : Singleton<GameManager> {
        #region Member
        [Title("UI")]
        [SerializeField]
        GameProgressUI progressUI;
        [SerializeField]
        GameOverUI gameOverUI;

        public int Score { get; private set; } = 0;
        #endregion

        #region Listener
        public Action<int> OnScoreChange { get; set; }
        public Action<bool> OnGameOver { get; set; }
        #endregion


        private void Start() {
            OnScoreChange += (score) => { Score += score; };
        }
    }
}
