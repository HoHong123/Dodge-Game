using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


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
    public partial class PlayerController : MonoBehaviour {
        [Title("Status")]
        [PropertyRange(0, 10)]
        [SerializeField]
        float moveSpeed = 0;

        [Title("Physic")]
        [SerializeField]
        Rigidbody2D r2d;
        [SerializeField]
        CapsuleCollider2D collider;

        private Vector2 moveInput;


        public void OnMoveEvent(InputAction.CallbackContext context) {
            moveInput = context.ReadValue<Vector2>();

            if (context.canceled) {
                moveInput = Vector2.zero;
            }
        }

        private void FixedUpdate() {
            r2d.linearVelocity = new Vector2(moveInput.x * moveSpeed, r2d.linearVelocity.y);
        }
    }
}
