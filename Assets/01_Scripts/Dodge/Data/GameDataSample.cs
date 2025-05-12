using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Logger;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

using FilePathAttribute = Sirenix.OdinInspector.FilePathAttribute;

namespace Dodge.Data {
    [Serializable]
    public class GameDataSample : ScriptableObject {
        #region Classes
        [Serializable]
        public class Monster {
            [HorizontalGroup("Monster", 120f)]
            [BoxGroup("Monster/Icon")]
            [FilePath(AbsolutePath = false, ParentFolder = "")]
            [SerializeField, HideLabel]
            private string iconPath;
#if UNITY_EDITOR
            [BoxGroup("Monster/Icon")]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 80f)]
            [ShowInInspector, HideLabel]
            private Sprite IconThumbnail {
                get {
                    if (string.IsNullOrEmpty(iconPath))
                        return null;
                    try {
                        var icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
                        return icon;
                    }
                    catch (NullReferenceException ex) {
                        //HLogger.Exception(ex);
                        return default;
                    }
                }
            }
#endif

            [HorizontalGroup("Monster", 120f)]
            [BoxGroup("Monster/Prefab")]
            [FilePath(AbsolutePath = false, ParentFolder = "")]
            [SerializeField, HideLabel]
            private string prefabPath;
#if UNITY_EDITOR
            [BoxGroup("Monster/Prefab")]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 80f)]
            [ShowInInspector, HideLabel]
            private Texture2D PrefabThumbnail {
                get {
                    if (string.IsNullOrEmpty(prefabPath))
                        return null;
                    try {
                        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                        return UnityEditor.AssetPreview.GetAssetPreview(prefab.gameObject);
                    }
                    catch (NullReferenceException ex) {
                        //HLogger.Exception(ex);
                        return default;
                    }
                }
            }
#endif

            [BoxGroup("Monster/Info")]
            [SerializeField]
            private string id;
            [BoxGroup("Monster/Info")]
            [SerializeField]
            private float hp;
            [BoxGroup("Monster/Info")]
            [SerializeField]
            private float atk;

            public string Id => id;
            public float Hp => hp;
            public float Atk => atk;
            public string IconPath => iconPath;
            public string PrefabPath => prefabPath;

            public Monster(string id, float hp, float atk, string iconPath, string prefabPath) {
                this.id = id;
                this.hp = hp;
                this.atk = atk;
                this.iconPath = iconPath;
                this.prefabPath = prefabPath;
            }
        }

        [System.Serializable]
        public class Reward {
            [SerializeField]
            private string id;
            [SerializeField]
            private float value;

            public string Id => id;
            public float Value => value;

            public Reward(string id, float value) {
                this.id = id;
                this.value = value;
            }
        }
        #endregion

        #region Serialized Fields
        [SerializeField]
        private string id;
        [SerializeField]
        private bool isBoss;

        [SerializeField]
        private List<Monster> monsters;
        [SerializeField]
        private List<Reward> rewards;

        [SerializeField]
        private string description;
        [SerializeField]
        private float exp;
        [SerializeField]
        private string result;
        #endregion

        #region Getter
        public string Id => id;
        public bool IsBoss => isBoss;
        public List<Monster> Monsters => monsters;

        public List<Reward> Rewards => rewards;

        public string Description => description;
        public float Exp => exp;
        public string Result => result;
        #endregion

#if UNITY_EDITOR
        public static GameDataSample FromJson(JToken json, string iconFolderPath, string prefabFolderPath) {
            var data = new GameDataSample();

            data.id = json["id"].Value<string>();
            data.isBoss = Convert.ToBoolean(json["isBoss"].Value<string>());

            var monsterId = json["monsterId"].Value<string>().Split(new char[] { ';' });
            var hps = json["hp"].Value<string>().Split(new char[] { ';' }).Select(e => Convert.ToSingle(e.Trim())).ToArray();
            var atks = json["atk"].Value<string>().Split(new char[] { ';' }).Select(e => Convert.ToSingle(e)).ToArray();

            data.monsters = new List<Monster>();

            for (int i = 0; i < monsterId.Length; i++) {
                var id = monsterId[i];
                var hp = hps[i];
                var atk = atks[i];
                var iconPath = $"{iconFolderPath}/{id}.png";
                var prefabPath = $"{prefabFolderPath}/{id}.prefab";

                data.monsters.Add(new Monster(id, hp, atk, iconPath, prefabPath));
            }

            var rewardId = json["rewardId"].Value<string>().Split(new char[] { ';' });
            var rewardValue = json["rewardValue"].Value<string>().Split(new char[] { ';' }).Select(e => Convert.ToSingle(e)).ToArray();

            data.rewards = new List<Reward>();

            for (int i = 0; i < rewardId.Length; i++) {
                var id = rewardId[i];
                var value = rewardValue[i];

                data.rewards.Add(new Reward(id, value));
            }

            data.description = json["description"].Value<string>();
            data.exp = Convert.ToSingle(json["exp"].Value<string>());
            data.result = json["result"].Value<string>();

            return data;
        }

        public JObject ToJson() {
            var json = new JObject();

            json.Add("id", id);
            json.Add("isBoss", isBoss.ToString());

            json.Add("monsterId", string.Join(";", monsters.Select(e => e.Id)));
            json.Add("hp", string.Join(";", monsters.Select(e => e.Hp)));
            json.Add("atk", string.Join(";", monsters.Select(e => e.Atk)));
            json.Add("iconPath", string.Join(";", monsters.Select(e => e.IconPath)));
            json.Add("prefabPath", string.Join(";", monsters.Select(e => e.PrefabPath)));

            json.Add("rewardId", string.Join(";", rewards.Select(e => e.Id)));
            json.Add("rewardValue", string.Join(";", rewards.Select(e => e.Value)));

            json.Add("description", description);
            json.Add("exp", exp.ToString());
            json.Add("result", result);

            return json;
        }
#endif
    }
}
