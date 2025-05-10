using System;
using UnityEngine;


#if UNITY_EDITOR
namespace Util.CustomAttribute {
    [AttributeUsage(AttributeTargets.Field)]
    public class MultiTagSelectorAttribute : PropertyAttribute {
        public string Label;

        public MultiTagSelectorAttribute(string label = "Selectable Tags") {
            this.Label = label;
        }
    }
}
#endif