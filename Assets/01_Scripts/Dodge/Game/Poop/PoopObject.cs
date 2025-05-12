using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Util.CustomAttribute;


namespace Dodge.Game.Poop {
    public class PoopObject : MonoBehaviour, IPoolable {
        [Title("Damage Settings")]
        [SerializeField]
        float damage = 10;
        [MultiTagSelector("충돌 태그 선택")]
        [SerializeField]
        private List<string> validTags = new();


        public void OnSpawned() {
            gameObject.SetActive(true);
        }

        public void OnReturned() {
            gameObject.SetActive(false);
        }


        private void OnTriggerEnter2D(Collider2D collision) {
            if (validTags.Contains(collision.transform.tag)) {
                var damagable = collision.gameObject.GetComponent<IDamagable>();
                if (damagable != null) {
                    damagable.TakeDamage(damage);
                }

                PoopManager.Instance.OnPoopReturn(this);
            }
        }
    }
}
