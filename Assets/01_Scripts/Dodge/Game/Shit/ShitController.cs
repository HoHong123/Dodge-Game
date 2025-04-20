using UnityEngine;
using Util.Container;


namespace Dodge.Game.Shit {
    public class ShitController : MonoBehaviour {

        [SerializeField]
        Pooling<ShitObject> pool;
    }
}
