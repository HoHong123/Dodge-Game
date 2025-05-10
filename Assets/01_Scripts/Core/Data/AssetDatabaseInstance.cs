using System;
using System.Linq;
using UnityEngine;
using Util.Logger;
using Sirenix.OdinInspector;

namespace Core.Data {
    [Serializable]
    public abstract class AssetDatabaseInstance<T> : ScriptableObject
            where T : AssetDatabaseInstance<T>, new() {
#if UNITY_EDITOR
        private bool isLoadedFromAsset = true;
        public bool IsLoadedFromAsset  => isLoadedFromAsset;

        private static T instance = null;
        public static T Instance {
            get {
                if (instance == null) {
                    var guid = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name)).FirstOrDefault();
                    if (guid == default) {
                        instance = new T();
                        instance.isLoadedFromAsset = false;
                    }
                    else {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                        HLogger.Log($"Path :: {path}");
                        instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                    }

                    HLogger.Log($"Guid :: {guid}");
                }
                return instance;
            }
        }

        [InfoBox(
            "It is only generated once. After saving, the automatically generated file is loaded.\n" +
            "(최초 한 번만 생성합니다. 저장 이후엔 자동으로 생성된 파일을 불러옵니다.)")]
        [HideIf("isLoadedFromAsset")]
        [Button("Save Setting File", ButtonHeight = 50)]
        private void CreateAsset() {
            string path = UnityEditor.EditorUtility.SaveFilePanel("Create Assets", "Assets", typeof(T).Name, "asset");
            if (!path.StartsWith(Application.dataPath))
                return;
            path = path.Replace(Application.dataPath, "Assets");
            HLogger.Log(path);
            try {
                instance.isLoadedFromAsset = true;
                UnityEditor.AssetDatabase.CreateAsset(instance, path);
            }
            catch (Exception e) {
                HLogger.Exception(e);
                return;
            }
        }
#endif
    }
}
