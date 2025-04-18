using UnityEngine;

namespace Util.Sound {
    public class SoundConnector : MonoBehaviour {
        public void PlayClickSound() => SoundManager.Instance.PlayClickSound();
        public void PlayOneShot(int _index) => SoundManager.Instance.PlayOneShot(_index);
        public void PlayBGM(int _index) => SoundManager.Instance.PlayBGM(_index);
    }
}
