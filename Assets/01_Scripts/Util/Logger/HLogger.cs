using System;
using System.Collections.Generic;
using UnityEngine;
using Dodge.UI.Popup;


namespace Util.Logger {
    public class HLogger : MonoBehaviour {
        private static int MAX_QUE_SIZE = 1000;

        private static Queue<string> LOG_QUE = new Queue<string>();
        private static Queue<string> WARNING_QUE = new Queue<string>();
        private static Queue<string> ERROR_QUE = new Queue<string>();
        private static Queue<string> FATAL_QUE = new Queue<string>();

        private static string UtcNow => DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz");


        public static void Log(string message, bool popupActivate = false, GameObject target = null) {
            string log = $"@1 [{UtcNow}] {message}";

#if UNITY_EDITOR
            if (target == null) {
                Debug.Log(log);
            }
            else {
                Debug.Log(log, target);
            }
#endif
            if (popupActivate) {
                PopupManager.Instance.AddAlert("Log", message);
            }

            LOG_QUE.Enqueue(log);
            if (LOG_QUE.Count > MAX_QUE_SIZE)
                LOG_QUE.Dequeue();
        }

        public static void Warning(string message, bool popupActivate = false, GameObject target = null) {
            string log = $"@2 [{UtcNow}] {message}";

#if UNITY_EDITOR
            if (target == null) {
                Debug.LogWarning(log);
            }
            else {
                Debug.LogWarning(log, target);
            }
#endif
            if (popupActivate) {
                PopupManager.Instance.AddAlert("Warning", message);
            }

            WARNING_QUE.Enqueue(message);
            if (WARNING_QUE.Count > MAX_QUE_SIZE)
                WARNING_QUE.Dequeue();
        }

        public static void Error(string message, bool showPopup = false, string debug = "", GameObject target = null) {
            string log = 
                $"@3 [{UtcNow}] {message}\n" +
                $"@3 Debug :: {debug}";

#if UNITY_EDITOR
            if (target == null) {
                Debug.LogWarning(log);
            }
            else {
                Debug.LogWarning(log, target);
            }
#endif

            if (showPopup) {
                PopupManager.Instance.AddAlert("Error", message);
            }

            ERROR_QUE.Enqueue(message);
            if (ERROR_QUE.Count > MAX_QUE_SIZE)
                ERROR_QUE.Dequeue();

            // TODO :: Decide what to do with log stack
            // ...
        }

        public static void Exception(Exception ex) {
            string log = $"@4 [{UtcNow}] {ex.Message}";

#if UNITY_EDITOR
            Debug.LogException(ex);
#endif

            FATAL_QUE.Enqueue(log);
        }

        public static void Throw(Exception ex) {
            Exception(ex);
            throw ex;
        }
    }
}
