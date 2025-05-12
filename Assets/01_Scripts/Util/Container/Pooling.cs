using System.Collections.Generic;
using UnityEngine;


namespace Util.Container {
    public class Pooling<T> where T : Component {
        readonly T prefab;
        readonly Queue<T> pool = new();
        readonly Transform parent;

        public int AvaliableCount => pool.Count;


        public Pooling(T prefab, int initialSize = 10, Transform parent = null) {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++) {
                T obj = GameObject.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }


        public T Get() {
            if (pool.Count == 0) {
                _AddNewObjects(1);
            }

            T obj = pool.Dequeue();

            if (obj is IPoolable poolable) {
                poolable.OnSpawned();
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj) {
            if (obj is IPoolable poolable) {
                poolable.OnReturned();
            }

            pool.Enqueue(obj);
        }


        private void _AddNewObjects(int count) {
            for (int i = 0; i < count; i++) {
                T obj = GameObject.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }
}

#if UNITY_EDITOR
/* Dev comment
 * 
 */
#endif