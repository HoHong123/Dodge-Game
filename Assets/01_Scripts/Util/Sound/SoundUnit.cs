using System.Collections.Generic;

using UnityEngine;


namespace Util.Sound {
    /// <summary>
    /// SoundUnit���� ����� �� Ŭ�� ������.   
    /// ������ GUI ȯ�濡 �� ��������� ���.
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