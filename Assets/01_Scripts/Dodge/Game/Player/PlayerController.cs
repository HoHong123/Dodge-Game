using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Dodge.Game.Player {
    public class PlayerController : MonoBehaviour {
        [Title("Status")]
        [PropertyRange(0, 10)]
        [SerializeField]
        float moveSpeed = 0;

        [Title("Physic")]
        [SerializeField]
        Rigidbody2D r2d;
        [SerializeField]
        CapsuleCollider2D collider;


        public void OnMoveEvent(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();

            if (!context.performed) {
                input = Vector2.zero;
            }

            Debug.Log(input);
            r2d.AddForce(input * moveSpeed, ForceMode2D.Force);
        }
    }
}
