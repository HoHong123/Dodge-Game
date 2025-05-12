using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Dodge.Data {
    [Serializable]
    public class TestDataSample : ScriptableObject {
        #region Serialized Fields
        [SerializeField]
        private string id;
        [SerializeField]
        private string description;
        [SerializeField]
        private string result;
        #endregion

        #region Getter
        public string Id => id;
        public string Description => description;
        public string Result => result;
        #endregion

#if UNITY_EDITOR
        public static TestDataSample FromJson(JToken json) {
            var data = new TestDataSample();

            data.id = json["id"].Value<string>();
            data.description = json["description"].Value<string>();
            data.result = json["result"].Value<string>();

            return data;
        }

        public JObject ToJson() {
            var json = new JObject();

            json.Add("id", id);
            json.Add("description", description);
            json.Add("result", result);

            return json;
        }
#endif
    }
}
