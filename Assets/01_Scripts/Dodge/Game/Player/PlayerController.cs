using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Util.Logger;

namespace Dodge.Game.Player {
    /* Player Controller
     * 1. Check physic
     * 2. Control status (Hp, Speed, etc)
     * 3. Control movement
     * 4. Control animation
     * 5. Control effects
     * 6. Event handling
     * 7. Preset handling
     */
    public partial class PlayerController : MonoBehaviour, IDamagable {
        #region Member
        [Title("Status")]
        [PropertyRange(0, 100)]
        [SerializeField]
        float hp = 100;
        [PropertyRange(0, 10)]
        [SerializeField]
        float moveSpeed = 0;

        Vector2 moveInput;

        [Title("Physic")]
        [SerializeField]
        Rigidbody2D r2d;
        [SerializeField]
        CapsuleCollider2D collider;

        [Title("UI")]
        [SerializeField]
        Slider hpSlider;
        [SerializeField]
        Animator animation;
        #endregion

        #region Listener
        public Action OnStatusChange { get; set; }
        #endregion


        private void Start() {
            hpSlider.maxValue = hp;
            hpSlider.value = hp;
            GameManager.Instance.OnGameOver += _OnGameOver;
        }


        private void FixedUpdate() {
            r2d.linearVelocity = new Vector2(moveInput.x * moveSpeed, r2d.linearVelocity.y);
        }


        public void TakeDamage(float damage) {
            HLogger.Log($"Damage :: {damage}");
            GameManager.Instance.OnScoreChange?.Invoke(-(int)damage);

            if ((hp -= damage) <= 0) {
                GameManager.Instance.OnGameOver?.Invoke(false);
            }

            hpSlider.value = hp;
            animation.SetTrigger("Hit");
            OnStatusChange?.Invoke();
        }

        public void OnMoveEvent(InputAction.CallbackContext context) {
            moveInput = context.ReadValue<Vector2>();

            if (context.canceled) {
                moveInput = Vector2.zero;
            }
        }


        private void _OnGameOver(bool result) {
            r2d.linearVelocityX = 0;
            enabled = false;
        }
    }
}
