using System;
using System.Collections.Generic;
using UnityEngine;
using Util.Logger;
using Util.Container;
using Sirenix.OdinInspector;

namespace Dodge.UI.Popup {
    public class PopupManager : Singleton<PopupManager> {
        [Title("UI")]
        [SerializeField]
        GameObject background;
        [FoldoutGroup("UI/Popup")]
        [SerializeField]
        SimpleTextPopup simpleText;
        [FoldoutGroup("UI/Popup")]
        [SerializeField]
        AlertPopup alertPopup;

        [Title("Parents")]
        [SerializeField]
        Transform generalPopupParent;
        [SerializeField]
        Transform debugPopupParent;
        [SerializeField]
        Transform alertPopupParent;

        #region Alert
        Queue<(string, string)> alertPool;
        Pooling<AlertPopup> alertPopups;
        #endregion

        #region Listeners
        public Action<AlertPopup> OnAlertClose { get; private set; }
        #endregion


        private void Awake() {
            alertPool = new Queue<(string, string)>();
            alertPopups = new Pooling<AlertPopup>(alertPopup, 3, alertPopupParent);

            OnAlertClose += _OnAlertClose;
        }


        public void AddLog() {

        }

        public void AddAlert(string title, string message, bool debug = false) {
            alertPool.Enqueue((title, message));
            _ShowAlert();

#if UNITY_EDITOR
            if (debug) {
                HLogger.Error($"{title} :: {message}");
            }
#endif
        }


        private void _ShowAlert() {
            if (alertPool.Count < 1) return;

            var context = alertPool.Dequeue();
            var alert = alertPopups.Get();
            alert.Init(context.Item1, context.Item2);
            alert.gameObject.SetActive(true);
        }

        private void _OnAlertClose(AlertPopup popup) {
            popup.gameObject.SetActive(false);
            alertPopups.ReturnToPool(popup);
        }
    }
}