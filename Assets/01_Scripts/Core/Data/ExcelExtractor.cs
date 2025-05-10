using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util.Logger;
using NPOI.SS.UserModel;    // SpreadSheet common utility
using NPOI.HSSF.UserModel; // Excel 97~2003 format
using NPOI.XSSF.UserModel; // Excel 2007 after. OOXML format
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Core.Data {
    [Serializable]
    public abstract class ExcelExtractor<Loader> :
        AssetDatabaseInstance<Loader>
        where Loader : ExcelExtractor<Loader>, new() {
        [Sirenix.OdinInspector.FilePath(AbsolutePath = false, Extensions = "xls, xlsx")]
        [HorizontalGroup("Find")]
        [BoxGroup("Find/Excel Path")]
        [OnValueChanged("LoadExcelFile")]
        [SerializeField, HideLabel]
        private string excelPath;

        [HorizontalGroup("Find")]
        [BoxGroup("Find/Select Sheet")]
        [ValueDropdown("sheets")]
        [OnValueChanged("GetSheet")]
        [SerializeField, HideLabel]
        private string sheetName;

        private IWorkbook workBook; // Current Excel
        private ISheet sheet;             // Current Sheet

        private List<string> sheets {
            get {
                if (workBook == null)
                    return null;
                var sheets = new List<string>();
                for (int i = 0; i < workBook.NumberOfSheets; i++) {
                    sheets.Add(workBook.GetSheetName(i));
                }
                return sheets;
            }
        }

        private bool isAvailable {
            get {
                if (sheet == null)
                    return false;

                var cols = new List<string>();
                foreach (var col in sheet.GetRow(0)) {
                    var value = col.ToString();
                    cols.Add(value);
                }

                foreach (var key in keys) {
                    if (!cols.Contains(key))
                        return false;
                }

                return true;
            }
        }

        protected abstract string[] keys { get; }


        private void GetSheet() {
            if (workBook == null) // 엑셀 유무 확인
                return;

            if (string.IsNullOrEmpty(sheetName)) { // 타겟 시트 유무 확인
                sheet = null;
                return;
            }

            sheet = workBook.GetSheet(sheetName); // 타겟 시트 호출
        }
        public void SetDefaultExcelSettings(string filePath) {
            this.excelPath = filePath;
            LoadExcelFile();
        }


        [HorizontalGroup("File Control")]
        [Button("Load Excel", Style = (Sirenix.OdinInspector.ButtonStyle)ButtonStyle.Box, ButtonHeight = 50)]
        private void LoadExcelFile() {
            if (string.IsNullOrEmpty(excelPath)) {
                HLogger.Error("Execl path not found.");
                return;
            }

            try {
                var path = Path.Combine(Application.dataPath, excelPath);
                HLogger.Log(path);
                using (var fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read)) {
                    if (excelPath.EndsWith("xls")) {
                        this.workBook = new HSSFWorkbook(fs);
                    }
                    else if (excelPath.EndsWith("xlsx")) {
                        this.workBook = new XSSFWorkbook(fs);
                    }
                    else {
                        HLogger.Throw(new NotSupportedException());
                    }
                }
            }
            catch (Exception e) {
                HLogger.Throw(e);
            }

            GetSheet();
        }
        [ShowIf("isAvailable")]
        [HorizontalGroup("File Control")]
        [Button("Create Assets", Style = ButtonStyle.Box, ButtonHeight = 50)]
        public abstract void ImportData();
        [Button("Export to Excel", Style = ButtonStyle.Box, ButtonHeight = 50)]
        public abstract void ExportData();


        protected JArray ExcelToJson() {
            var arr = new JArray();
            var cols = new Dictionary<string, int>();
            var firstRow = sheet.GetRow(0);

            for (int i = 0; i < firstRow.LastCellNum; i++) {
                var value = firstRow.GetCell(i).ToString();
                cols.Add(value, i);
            }

            for (int i = 1; i <= sheet.LastRowNum; i++) {
                var row = sheet.GetRow(i);
                var json = new JObject();

                foreach (var col in cols) {
                    var cell = row.GetCell(col.Value);
                    if (cell != null && cell.CellType != CellType.Blank) {
                        // 데이터가 존재하지 않는 경우, json에 추가하지 않을 예정
                        var value = cell.ToString();
                        json.Add(col.Key, value);
                    }
                }

                arr.Add(json);
            }

            return arr;
        }

        protected void JsonToExcel(JArray jArray, string fileName = "") {
            IWorkbook book = new XSSFWorkbook();
            var sheet = book.CreateSheet("Sheet");
            IRow row = sheet.CreateRow(0);

            for (int i = 0; i < keys.Length; i++) {
                var key = keys[i];
                row.CreateCell(i).SetCellValue(key);
            }

            for (int i = 0; i < jArray.Count; i++) {
                var json = jArray[i] as JObject;

                row = sheet.CreateRow(i + 1);

                foreach (var property in json.Properties()) {
                    var key = property.Name;
                    var value = property.Value.Value<string>();

                    var index = Array.IndexOf(keys, key);
                    row.CreateCell(index).SetCellValue(value);
                }
            }

            string path = EditorUtility.SaveFilePanel("Convert to excel", "", fileName, "xlsx");

            HLogger.Log(path);

            using (var fs = new FileStream(path, FileMode.Create)) {
                book.Write(fs);
            }
        }
    }
}
