using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Util.Container;


namespace Dodge.Game.Shit {
    public class ShitController : MonoBehaviour {
        [Title("Shit")]
        [SerializeField]
        ShitObject shitPrefab;
        [SerializeField]
        Transform shitParent;

        Pooling<ShitObject> pool;

        public Action<ShitObject> OnReturn { get; private set; }


        private void Awake() {
            pool = new Pooling<ShitObject>(shitPrefab, 10, shitParent);
            OnReturn += _OnShitReturn;
        }


        private void _OnShitReturn(ShitObject shit) {
            pool.ReturnToPool(shit);
        }
    }
}
