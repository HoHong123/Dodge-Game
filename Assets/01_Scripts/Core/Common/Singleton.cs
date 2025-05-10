using UnityEngine;
using Sirenix.OdinInspector;
using Util.Logger;


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
#if UNITY_EDITOR
                    HLogger.Error($"Singletone issue!!! {typeof(T).Name} is not found. Auto create activated.");
                    instance.name = $"AutoCreate_{typeof(T).Name}";
#endif
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
