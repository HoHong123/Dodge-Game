using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Dodge.UI.Popup {
    public class SimpleTextPopup : MonoBehaviour {
        [Title("Texts")]
        [SerializeField]
        TMP_Text titleTxt;
        [SerializeField]
        TMP_Text descriptionTxt;

        [Title("UI")]
        [SerializeField]
        Button closeBtn;


        private void Start() {
            closeBtn.onClick.AddListener(OnClickClose);
        }


        public void Init(string title, string message) {
            titleTxt.text = title;
            descriptionTxt.text = message;
            closeBtn.interactable = true;
        }


        public void OnClickClose() {
            closeBtn.interactable = false;
            PopupManager.Instance.OnLogClose(this);
        }
    }
}