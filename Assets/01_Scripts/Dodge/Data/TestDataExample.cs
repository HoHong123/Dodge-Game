using System.IO;
using UnityEditor;
using UnityEngine;
using Core.Data;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Dodge.Data {
    public class TestDataExample : ExcelExtractor<TestDataExample> {
        [FolderPath(AbsolutePath = false)]
        [HorizontalGroup("Group")]
        [BoxGroup("Group/Scriptables Path")]
        [SerializeField, HideLabel]
        private string scriptablesPath; // Scriptable datas path
        public string ScriptablesPath => scriptablesPath;

        protected override string[] keys =>
            new string[] {
                    "id",
                    "description",
                    "result",
            };


        public override void ImportData() {
            var arr = ExcelToJson();

            foreach (var json in arr) {
                var data = TestDataSample.FromJson(json);
                AssetDatabase.CreateAsset(data, $"{ScriptablesPath}/{data.Id}.asset");
            }
        }

        public override void ExportData() {
            var arr = new JArray();
            var files = Directory.GetFiles(ScriptablesPath, "*.asset", SearchOption.TopDirectoryOnly);

            foreach (var file in files) {
                var sample = AssetDatabase.LoadAssetAtPath<TestDataSample>(file);
                arr.Add(sample.ToJson());
            }

            JsonToExcel(arr, "TestDataSample");
        }
    }
}
