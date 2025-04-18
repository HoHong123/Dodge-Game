using System.Collections.Generic;

using UnityEngine;


namespace Util.Sound {
    /// <summary>
    /// SoundUnit에서 사용할 각 클립 데이터.   
    /// 에디터 GUI 환경에 각 멤버변수를 출력.
    /// </summary>
    [System.Serializable]
    public class SoundContainer {
        public int IntegerFlag;
        public SFXList EnumFlag;
        public AudioClip Clip;
    }

    public class SoundUnit : MonoBehaviour {
        [Header("Clip informations")]
        public List<SoundContainer> ClipList = new List<SoundContainer>();


        private void Awake() {
            SoundManager.Instance.SetSoundUnit(this);
        }

        private void OnDestroy() {
            if (SoundManager.Instance != null)
                SoundManager.Instance.RemoveSoundUnit(this);
        }
    }

}