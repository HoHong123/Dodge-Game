using System.Collections.Generic;
using UnityEngine;


namespace Util.Container {
    public class Pooling<T> where T : Component {
        private readonly T prefab;
        private readonly Queue<T> pool = new Queue<T>();
        private readonly Transform parent;

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
                AddObjects(1);
            }

            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj) {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        private void AddObjects(int count) {
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