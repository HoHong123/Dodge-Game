using System;
using UnityEngine;
using Util.Container;
using Sirenix.OdinInspector;

namespace Dodge.Game.Poop {
    public class PoopManager : Singleton<PoopManager> {
        #region Member
        [Title("Pooling")]
        [SerializeField]
        PoopObject poopPrefab;
        [SerializeField]
        Transform poopParent;

        Pooling<PoopObject> pool;

        [TitleGroup("Spawn")]
        [SerializeField, ReadOnly]
        float cooldown;
        [TitleGroup("Spawn")]
        [SerializeField]
        float cooldownDrain = 0.99f;
        [TitleGroup("Spawn")]
        [MinMaxSlider(0, 5, true)]
        [SerializeField]
        Vector2 cooldownRange = new(0.5f, 1f);

        float timer;

        [HorizontalGroup("Spawn/Point")]
        [SerializeField]
        Vector2 pointA;
        [HorizontalGroup("Spawn/Point")]
        [SerializeField]
        Vector2 pointB;
        #endregion

        #region Listener
        public Action<PoopObject> OnPoopReturn { get; private set; }
        #endregion


        private void Awake() {
            pool = new Pooling<PoopObject>(poopPrefab, 20, poopParent);
            OnPoopReturn += _OnPoopReturn;
        }

        private void Start() {
            GameManager.Instance.OnGameOver += (result) => { enabled = false; };
        }

        private void Update() {
            timer += Time.deltaTime;
            if (timer >= cooldown && pool.AvaliableCount > 0) {
                var poop = pool.Get();
                poop.transform.localPosition = _CalcRandomPosition();

                timer = 0f;
                cooldown = UnityEngine.Random.Range(cooldownRange.x, cooldownRange.y);
                cooldownRange *= cooldownDrain;

                GameManager.Instance.OnScoreChange?.Invoke(20);
            }
        }


        private void _OnPoopReturn(PoopObject poop) {
            pool.ReturnToPool(poop);
        }

        private Vector2 _CalcRandomPosition() {
            float x = UnityEngine.Random.Range(Mathf.Min(pointA.x, pointB.x), Mathf.Max(pointA.x, pointB.x));
            float y = UnityEngine.Random.Range(Mathf.Min(pointA.y, pointB.y), Mathf.Max(pointA.y, pointB.y));
            return new Vector2(x, y);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Vector3 min = new Vector3(Mathf.Min(pointA.x, pointB.x), Mathf.Min(pointA.y, pointB.y), 0f);
            Vector3 max = new Vector3(Mathf.Max(pointA.x, pointB.x), Mathf.Max(pointA.y, pointB.y), 0f);
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointA, 0.05f);
            Gizmos.DrawSphere(pointB, 0.05f);
        }
#endif
    }
}
