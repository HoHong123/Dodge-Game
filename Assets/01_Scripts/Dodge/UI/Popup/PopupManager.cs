using System;
using System.Collections.Generic;
using UnityEngine;
using Util.Logger;
using Util.Container;
using Sirenix.OdinInspector;

namespace Dodge.UI.Popup {
    public class PopupManager : Singleton<PopupManager> {
        #region Member
        [TitleGroup("UI")]
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
        Transform logParent;
        [SerializeField]
        Transform warningParent;
        [SerializeField]
        Transform errorParent;
        [SerializeField]
        Transform fatalParent;

        [Title("Logs")]
        [SerializeField]
        Queue<(string, string)> logPool = new();
        [SerializeField]
        Queue<(string, string)> warningPool = new();
        [SerializeField]
        Queue<(string, string)> alertPool = new();
        [SerializeField]
        Queue<(string, string)> fatalPool = new();

        Pooling<SimpleTextPopup> logPopups;
        Pooling<AlertPopup> alertPopups;

        bool isAllClose => 
            (logPool.Count + warningPool.Count + alertPool.Count + fatalPool.Count) == 0 &&
            (logPopups.AvaliableCount + alertPopups.AvaliableCount) == 4;
        #endregion

        #region Listeners
        public Action<SimpleTextPopup> OnLogClose { get; private set; }
        public Action<AlertPopup> OnAlertClose { get; private set; }
        #endregion


        protected override void Awake() {
            logPopups = new Pooling<SimpleTextPopup>(simpleText, 2, logParent);
            alertPopups = new Pooling<AlertPopup>(alertPopup, 2, errorParent);

            OnLogClose += _OnLogClose;
            OnAlertClose += _OnAlertClose;

            base.Awake();
        }


        public void AddLog(string title, string message) {
            background.SetActive(true);

            HLogger.Log($"{title} :: {message}");
            logPool.Enqueue((title, message));
            _ShowLog();
        }

        public void AddAlert(string title, string message, bool debug = false) {
            background.SetActive(true);

            alertPool.Enqueue((title, message));
            _ShowAlert();

            if (debug) {
                HLogger.Error($"{title} :: {message}");
            }
        }


        private void _ShowLog() {
            if (logPopups.AvaliableCount < 1) return;
            if (logPool.Count < 1) return;

            var context = logPool.Dequeue();
            var log = logPopups.Get();
            log.Init(context.Item1, context.Item2);
            log.gameObject.SetActive(true);
            log.transform.SetAsFirstSibling();
        }

        private void _ShowAlert() {
            if (alertPopups.AvaliableCount < 1) return;
            if (alertPool.Count < 1) return;

            var context = alertPool.Dequeue();
            var alert = alertPopups.Get();
            alert.Init(context.Item1, context.Item2);
            alert.gameObject.SetActive(true);
            alert.transform.SetAsFirstSibling();
        }

        private void _OnLogClose(SimpleTextPopup popup) {
            popup.gameObject.SetActive(false);
            logPopups.ReturnToPool(popup);
            _ShowLog();
            background.SetActive(!isAllClose);
        }

        private void _OnAlertClose(AlertPopup popup) {
            popup.gameObject.SetActive(false);
            alertPopups.ReturnToPool(popup);
            _ShowAlert();
            background.SetActive(!isAllClose);
        }
    }
}