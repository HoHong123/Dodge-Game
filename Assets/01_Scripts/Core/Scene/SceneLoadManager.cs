using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Logger;
using Sirenix.OdinInspector;


namespace Core.Scene {
    public class SceneLoadManager : Singleton<SceneLoadManager> {
        [Title("Payload")]
        [SerializeField]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        private Dictionary<string, object> payloadDic = new();

        public Action StartSceneLoad { get; set; }
        public Action EndOnSceneLoad { get; set; }


        #region Payload
        public T GetPayload<T>(string key, bool removeAfter = false) where T : class {
            object payload = payloadDic[key];

            if (payload.GetType() != typeof(T)) {
                HLogger.Error(
                    "Payload casting fail",
                    false,
                    $"Payload Type [{payload.GetType()}]\n, Input Type [{typeof(T).FullName}]");
                return null;
            }

            if (removeAfter) {
                RemovePayload(key);
            }

            return (T)payload;
        }
        public void SavePayload<T>(string payloadKey, T payload) {
            if (payloadDic.ContainsKey(payloadKey)) {
                HLogger.Error($"{payloadKey} is already exist.");
                return;
            }
            payloadDic.Add(payloadKey, payload);
        }
        public void RemovePayload(string payloadKey) {
            if (!payloadDic.ContainsKey(payloadKey)) return;
            payloadDic[payloadKey] = null;
            payloadDic.Remove(payloadKey);
        }
        #endregion

        #region Load Scene
        public void LoadSceneAsync(SceneList sceneList) => LoadSceneAsync((int)sceneList);
        public void LoadSceneAsync(string sceneName) {
            if (Enum.IsDefined(typeof(SceneList), sceneName)) {
                HLogger.Exception(new IndexOutOfRangeException(), "SceneLoadManager");
                return;
            }
            LoadSceneAsync((int)Enum.Parse(typeof(SceneList), sceneName));
        }
        public void LoadSceneAsync(int sceneList) {
            _LoadScene(sceneList);
        }

        private async void _LoadScene(int sceneList) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneList);

            StartSceneLoad?.Invoke();

            await UniTask.WaitUntil(() => asyncLoad.isDone);
            HLogger.Log($"Scene loaded [{sceneList.ToString()}]");

            EndOnSceneLoad?.Invoke();

            await Resources.UnloadUnusedAssets();
            HLogger.Log("Scene asset clear complete");
        }
        #endregion
    }
}