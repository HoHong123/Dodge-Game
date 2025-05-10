using Sirenix.OdinInspector;
using UnityEngine;
using Util.Container;


namespace Dodge.Game.Shit {
    public class ShitController : MonoBehaviour {
        [Title("Shit")]
        [SerializeField]
        ShitObject shitPrefab;
        [SerializeField]
        Transform shitParent;

        Pooling<ShitObject> pool;


        private void Awake() {
            pool = new Pooling<ShitObject>(shitPrefab, 10, shitParent);
        }
    }
}
