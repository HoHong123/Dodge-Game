using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Util.CustomAttribute;


namespace Dodge.Game.Shit {
    public class ShitObject : MonoBehaviour, IPoolable {
        [Title("Damage Settings")]
        [MultiTagSelector("충돌 태그 선택")]
        [SerializeField]
        private List<string> validTags = new();

        public void OnSpawned() {
        }

        public void OnReturned() {
        }

        private void DealDamage() {
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var tag = collision.transform.tag;
            switch (tag) {
            case "Player": DealDamage(); break;
            }
        }
    }
}
