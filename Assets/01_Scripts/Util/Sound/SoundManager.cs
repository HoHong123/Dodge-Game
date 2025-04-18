using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Util.Logger;


namespace Util.Sound {
    /// <summary>
    /// ���� ��η� �����ϴ� SoundUnit�� Ŭ�� ������ ȿ�������� �����ϱ� ���� Ŭ����
    /// </summary>
    public class SoundItem {
        public int Dependency;
        public AudioClip Clip;

        public SoundItem(int _dependency, AudioClip _clip) {
            Dependency = _dependency;
            Clip = _clip;
        }
    }

    public class SoundManager : Singleton<SoundManager> {
        [Title("Audio Mixer")]
        [SerializeField] private AudioMixer audioMix;
        [SerializeField] private AudioSource sfxAudio;
        [SerializeField] private AudioSource bgmAudio;

        [Title("Delay Play")]
        [PropertyRange(0, 1)]
        [SerializeField]
        private float audioDelay;

        private bool isDelayOver = true;
        private Queue<int> delayClipQue = new Queue<int>();

        [Title("Sound Clip")]
        [SerializeField]
        private Dictionary<int, SoundItem> soundDic = new Dictionary<int, SoundItem>();


        public void SetSoundUnit(SoundUnit _unit) {
            foreach (var container in _unit.ClipList) {
                if (container.Clip == null) {
                    HLogger.Error(
                        $"SetSoundUnit(SoundManager)\n" +
                        $"{_unit.gameObject.name} object is trying to enter null clip.\n" +
                        $"Object name = {_unit.gameObject.name}, Enum = {container.EnumFlag}, Integer Flag = {container.IntegerFlag}"
                        );
                    continue;
                }

                if (soundDic.ContainsKey(container.IntegerFlag)) {
                    soundDic[container.IntegerFlag].Dependency++;
                    continue;
                }

                soundDic.Add(container.IntegerFlag, new SoundItem(1, container.Clip));
            }

#if UNITY_EDITOR
            _ShowSoundList();
#endif
        }

        public void RemoveSoundUnit(SoundUnit _unit) {
            if (soundDic == null) {
                HLogger.Error("Sound List is null");
                return;
            }

            foreach (var container in _unit.ClipList) {
                if (!soundDic.ContainsKey(container.IntegerFlag) ||
                    --soundDic[container.IntegerFlag].Dependency > 0)
                    continue;

                soundDic.Remove(container.IntegerFlag);
            }

#if UNITY_EDITOR
            _ShowSoundList();
#endif
        }

        /// <summary> ���� ���� ���Ǵ� ���忡 ���� Ư�� ���̽��� �ν��Ͽ� ���� �Լ��� �ۼ� </summary>
        public void PlayClickSound() => PlayOneShot((int)SFXList.Notification);
        public void PlayOneShot(SFXList _sfx) => PlayOneShot((int)_sfx);
        public void PlayOneShotWithDelay(SFXList _sfx) => PlayOneShotWithDelay((int)_sfx);


        public void PlayOneShot(int _index) {
            if (!_CanPlayClip(_index))
                return;
            sfxAudio.PlayOneShot(soundDic[_index].Clip);
        }

        public void PlayOneShotWithDelay(int _index) {
            delayClipQue.Enqueue(_index);
            if (!isDelayOver || !_CanPlayClip(_index))
                return;

            sfxAudio.PlayOneShot(soundDic[delayClipQue.Dequeue()].Clip);
            _StartDelay();
        }

        public void PlayBGM(int _index) {
            if (!_CanPlayClip(_index))
                return;
            // BGM�� �ʿ��� ��� ���� �Է�
        }


        /// <summary>
        /// ���带 ����� �� �ִ� �������� Ȯ���ϴ� �Լ�
        /// </summary>
        /// <param name="_index">��ǥ ���� Ŭ��</param>
        /// <returns>���� ��</returns>
        private bool _CanPlayClip(int _index) {
            //if (!SettingManager.Instance.SFX) return false;
            if (!soundDic.ContainsKey(_index)) {
                HLogger.Error($"Cannot play sound. Does not have {_index} sound data.");
                return false;
            }

            return true;
        }

        private async void _StartDelay() {
            isDelayOver = false;
            await UniTask.WaitForSeconds(audioDelay);
            isDelayOver = true;
        }

#if UNITY_EDITOR
        private void _ShowSoundList() {
            HLogger.Log(
                $"Current Sound Clip Elements.\n" +
                string.Join("\n", soundDic.Select(kv => $"{kv.Key}: Dependency = {kv.Value.Dependency}, Clip = {kv.Value.Clip.name}")));
        }
#endif
    }
}

#if UNITY_EDITOR
/* - ���� �α� -
 * ==========================================================
 * Dictionary�� ���� ������ ���� ���ҽ��� �ʿ信 ���� O(1)�� �ӵ��� ������ ������ �ʿ䰡 �ִٰ� �����Ͽ� �ۼ��Ͽ����ϴ�.
 * Serializable Dictionary�� ���� �ν����� GUI���� ������ ������ �����ϱ� ���� ����� �õ��� �Ͽ����ϴ�.
 * ������ ������� ����ġ�� Serializable Dictionary ������� ��ǻ� ����Ʈ 2���� ����� ���������� �� �������� ����ϴ� ���� �����ϴ�.
 * �Ͽ� ����� �ʿ信 ���� ���� ������ ���Ŀ� �����Ͽ��� ����� �����ϴ�.
 * ==========================================================
 * ���� ���� Ŭ���� SoundUnit ������Ʈ�� �����ϴ� ��쿡�� �߰��ǰ� �ش� ������Ʈ�� �ı��Ǹ� ������ ���� Ŭ���� �����ϴ� ����Դϴ�.
 * �̷��� ����� ������ ���� ũ���� ����ϴ� ������ �ߺ��Ǿ� ������ ���, �ϳ��� ������ �ı��� ��, �������� ����ϴ� Ŭ���� ���ŵǴ� ������ �߻��� �� �ֽ��ϴ�. �Ͽ� ���Ӽ��� ī��Ʈ�ϴ� ������ �ֱ�� �Ͽ����ϴ�.
 * ���� ���� ����� �־�����, ��ųʸ� ���ο� ���ӵǴ� ������ ���� ī��Ʈ �� �� �ִ� Ŭ������ ���� ����� ���� ���� �������� ���� ���̶� �����Ͽ� SoundItem Ŭ������ ��������ϴ�.
 * ==========================================================
 */
#endif