using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Dodge.UI.Popup {
    public class AlertPopup : MonoBehaviour {
        [Title("Texts")]
        [SerializeField]
        TMP_Text titleTxt;
        [SerializeField]
        TMP_Text descriptionTxt;

        [Title("UI")]
        [SerializeField]
        Button closeBtn;


        public void Init(string title, string message) {
            titleTxt.text = title;
            descriptionTxt.text = message;

            closeBtn.interactable = true;
            transform.DOScale(1, 0.6f).SetEase(Ease.InOutExpo);
        }

        public void OnClickClose() {
            closeBtn.interactable = false;
            transform.
                DOScale(0, 0.6f).
                SetEase(Ease.InOutExpo).
                OnComplete(() => {
                    PopupManager.Instance.OnAlertClose(this);
                });
        }
    }
}