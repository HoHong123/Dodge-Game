using Core.Scene;
using Dodge.UI.Popup;
using UnityEngine;

namespace Dodge.Lobby {
    public class LobbyManager : MonoBehaviour {
        public void OnClickStart() {
            SceneLoadManager.Instance.LoadSceneAsync(SceneList.Game);
        }
    }
}
