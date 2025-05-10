using System.IO;
using UnityEditor;
using UnityEngine;
using Core.Data;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Dodge.Data {
    public class GameDataExample : ExcelExtractor<GameDataExample> {
        [FolderPath(AbsolutePath = false)]
        [HorizontalGroup("Group")]
        [BoxGroup("Group/Sample Scriptables Path")]
        [SerializeField, HideLabel]
        private string scriptablesPath; // Scriptable datas path
        public string ScriptablesPath => scriptablesPath;

        [FolderPath]
        [BoxGroup("Icon Path")]
        [SerializeField, HideLabel]
        private string iconPath; // Icon resources path
        public string IconPath => iconPath;

        [FolderPath]
        [BoxGroup("Prefab Path")]
        [SerializeField, HideLabel]
        private string prefabPath; // Prefab resources path
        public string PrefabPath => prefabPath;

        protected override string[] keys => 
            new string[] {
                    "id",
                    "isBoss",
                    "monsterId",
                    "hp",
                    "atk",
                    "rewardId",
                    "rewardValue",
                    "description",
                    "exp",
                    "result",
                    "iconPath",
                    "prefabPath"
            };


        public override void ImportData() {
            var arr = ExcelToJson();

            foreach (var json in arr) {
                var data = GameDataExampleSample.FromJson(json, iconPath, prefabPath);
                AssetDatabase.CreateAsset(data, $"{ScriptablesPath}/{data.Id}.asset");
            }
        }

        public override void ExportData() {
            var arr = new JArray();
            var files = Directory.GetFiles(ScriptablesPath, "*.asset", SearchOption.TopDirectoryOnly);

            foreach (var file in files) {
                var sample = AssetDatabase.LoadAssetAtPath<GameDataExampleSample>(file);
                arr.Add(sample.ToJson());
            }

            JsonToExcel(arr, "ExampleSample");
        }
    }
}
