using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Util.Logger;


namespace Util.Sound {
    /// <summary>
    /// 여러 경로로 접근하는 SoundUnit의 클립 정보를 효율적으로 저장하기 위한 클래스
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

        /// <summary> 가장 많이 사용되는 사운드에 대해 특수 케이스로 인식하여 전용 함수를 작성 </summary>
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
            // BGM이 필요한 경우 로직 입력
        }


        /// <summary>
        /// 사운드를 출력할 수 있는 상태인지 확인하는 함수
        /// </summary>
        /// <param name="_index">목표 사운드 클립</param>
        /// <returns>상태 값</returns>
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
/* - 개발 로그 -
 * ==========================================================
 * Dictionary를 통해 수많은 사운드 리소스를 필요에 따라 O(1)의 속도로 빠르게 접근할 필요가 있다고 생각하여 작성하였습니다.
 * Serializable Dictionary를 통해 인스펙터 GUI에서 데이터 포맷을 관리하기 쉽게 만드는 시도를 하였습니다.
 * 하지만 현재까지 리서치한 Serializable Dictionary 방법들은 사실상 리스트 2개를 사용한 선형구조를 눈 속임으로 사용하는 것이 었습니다.
 * 하여 충분히 필요에 따라 내부 로직을 추후에 변경하여도 상관이 없습니다.
 * ==========================================================
 * 현재 사운드 클립은 SoundUnit 컨포넌트가 존재하는 경우에만 추가되고 해당 컨포넌트가 파괴되면 연관된 사운드 클립을 제거하는 방식입니다.
 * 이러한 방식은 동일한 사운드 크립을 사용하는 유닛이 중복되어 생성될 경우, 하나의 유닛이 파괴될 때, 공통으로 사용하는 클립이 제거되는 문제가 발생할 수 있습니다. 하여 종속성을 카운트하는 로직을 넣기로 하였습니다.
 * 여러 접근 방식이 있었지만, 딕셔너리 내부에 종속되는 유닛의 수를 카운트 할 수 있는 클래스를 따로 만드는 것이 가장 가독성이 좋을 것이라 생각하여 SoundItem 클래스를 만들었습니다.
 * ==========================================================
 */
#endif