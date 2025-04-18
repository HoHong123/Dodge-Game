using Sirenix.OdinInspector;
using UnityEngine;


public class Singleton<T> : SerializedMonoBehaviour where T : MonoBehaviour {
    [Title("Don't Destroy")]
    [SerializeField]
    bool dontDestroy;

    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = FindFirstObjectByType<T>();
                if (instance == null) {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    instance.name = $"AutoCreate_{typeof(T).Name}";
                }
            }

            return instance;
        }
    }

    private void Awake() {
        if (dontDestroy) {
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}
