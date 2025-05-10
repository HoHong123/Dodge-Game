#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using Dodge.Data;

namespace Core.Data {
    public class DataEditorWindow : OdinMenuEditorWindow {
        [MenuItem("Game Data/Data Editor")]
        private static void OpenWindow() {
            var window = GetWindow<DataEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 700);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();

            tree.Add("1. Sample", GameDataExample.Instance);
            tree.AddAllAssetsAtPath("1. Sample", GameDataExample.Instance.ScriptablesPath, typeof(GameDataExampleSample), true, true);

            tree.SortMenuItemsByName(false);

            tree.Config.DrawSearchToolbar = true;

            return tree;
        }
    }
}
#endif