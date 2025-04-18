using System.Collections.Generic;
using UnityEngine;


namespace Util.Logger {
    public class HLogger : MonoBehaviour {
        private static int MAX_LOG_SIZE = 1000;

        private static Queue<string> LOG_QUE = new Queue<string>();
        private static Queue<string> WARNING_QUE = new Queue<string>();
        private static Queue<string> ERROR_QUE = new Queue<string>();


        public static void Log(string message, bool popupActivate = false, GameObject target = null) {
#if UNITY_EDITOR
            if (target == null) {
                Debug.Log($"@ LOG:: {message}");
            }
            else {
                Debug.Log($"@ LOG:: {message}", target);
            }
#endif
            if (popupActivate) {
                //PopupManager.Instance.AlertPopup("Error", message);
            }

            LOG_QUE.Enqueue(message);
            if (LOG_QUE.Count > MAX_LOG_SIZE)
                LOG_QUE.Dequeue();
        }

        public static void Warning(string message, bool popupActivate = false, GameObject target = null) {
#if UNITY_EDITOR
            if (target == null) {
                Debug.LogWarning($"@@ WARN:: {message}");
            }
            else {
                Debug.LogWarning($"@@ WARN:: {message}", target);
            }
#endif
            if (popupActivate) {
                //PopupManager.Instance.AlertPopup("Error", message);
            }

            WARNING_QUE.Enqueue(message);
            if (WARNING_QUE.Count > MAX_LOG_SIZE)
                WARNING_QUE.Dequeue();
        }

        public static void Error(string message, string debug = "", bool popupActivate = false, GameObject target = null) {
#if UNITY_EDITOR
            if (target == null) {
                Debug.LogWarning(
                    $"@@@ ERROR:: {message}\n" +
                    $"@@@ Debug:: {debug}"
                );
            }
            else {
                Debug.LogWarning(
                    $"@@@ ERROR:: {message}\n" +
                    $"@@@ Debug:: {debug}"
                    , target
                );
            }
#endif
            if (popupActivate) {
                //PopupManager.Instance.AlertPopup("Error", message);
            }

            ERROR_QUE.Enqueue(message);
            if (ERROR_QUE.Count > MAX_LOG_SIZE)
                ERROR_QUE.Dequeue();

            // TODO - Decide what to do with log stack
            // ...
        }
    }
}
