using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;


#if UNITY_EDITOR
namespace Util.CustomAttribute {
    public class MultiTagSelectorDrawer : OdinValueDrawer<List<string>> {
        private bool foldout = false;

        protected override void DrawPropertyLayout(GUIContent label) {
            string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;
            var currentList = ValueEntry.SmartValue;

            var attr = this.Property.GetAttribute<MultiTagSelectorAttribute>();
            string title = string.IsNullOrEmpty(attr.Label) ? label.text : attr.Label;

            SirenixEditorGUI.BeginBox();
            foldout = SirenixEditorGUI.Foldout(foldout, title);

            if (foldout) {
                EditorGUI.indentLevel++;
                foreach (string tag in allTags) {
                    bool isSelected = currentList.Contains(tag);
                    bool newSelected = EditorGUILayout.ToggleLeft(tag, isSelected);

                    if (newSelected && !isSelected) {
                        currentList.Add(tag);
                    }
                    else if (!newSelected && isSelected) {
                        currentList.Remove(tag);
                    }
                }
                EditorGUI.indentLevel--;
            }

            SirenixEditorGUI.EndBox();

            ValueEntry.SmartValue = currentList;
        }
    }
}
#endif