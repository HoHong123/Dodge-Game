using Dodge.UI.Popup;
using UnityEngine;

public class PopupTest : MonoBehaviour {
    int lcount = 0;
    int acount = 0;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            PopupManager.Instance.AddAlert($"Test {++acount}", $"Testing {acount}");
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            PopupManager.Instance.AddLog($"Test {++lcount}", $"Testing {lcount}");
        }
    }
}
