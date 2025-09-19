using System;
using System.Collections;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 個々のVFX再生を管理し、自身のライフサイクルを責務に持つコンポーネント。
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class VfxPlayer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;
        private Action<VfxPlayer> _onFinishedCallback;
        private Coroutine _playCoroutine;
        private SoundManager _soundManager;



        /// <summary>
        /// 現在再生中のVFX定義を取得します。
        /// </summary>
        public VfxDefinition VfxDefinition { get; private set; }

        /// <summary>
        /// このVFXプレイヤーを初期化します。
        /// </summary>
        /// <param name="onFinishedCallback">再生完了時に呼び出されるコールバック。</param>
        public void Initialize(Action<VfxPlayer> onFinishedCallback, SoundManager soundManager)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
            _onFinishedCallback = onFinishedCallback;
            _soundManager = soundManager;
            _audioSource.outputAudioMixerGroup = _soundManager.SEGroup;
        }

        /// <summary>
        /// 指定された定義に基づいてVFXを再生します。
        /// </summary>
        /// <param name="vfxDefinition">再生するVFXの定義。</param>
        public void Play(VfxDefinition vfxDefinition)
        {
            if (vfxDefinition == null)
            {
                Debug.LogError("VfxDefinition is null.");
                Stop();
                return;
            }
            this.VfxDefinition = vfxDefinition;

            // AudioSourceの設定
            _audioSource.clip = vfxDefinition.SEData.AudioClip;
            _audioSource.loop = vfxDefinition.SEData.IsLoop;
            if (_audioSource.clip != null)
            {
                _audioSource.Play();
            }

            // ParticleSystemの再生
            _particleSystem.Play();

            if (!vfxDefinition.IsLooping)
            {
                float duration = vfxDefinition.Duration > 0 ? vfxDefinition.Duration : _particleSystem.main.duration;
                _playCoroutine = StartCoroutine(WaitForCompletion(duration));
            }
        }

        /// <summary>
        /// VFXの再生を停止し、プールへの返却を通知します。
        /// </summary>
        public void Stop()
        {
            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
                _playCoroutine = null;
            }

            if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
            }

            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _onFinishedCallback?.Invoke(this);
        }

        private IEnumerator WaitForCompletion(float duration)
        {
            yield return new WaitForSeconds(duration);
            _playCoroutine = null;
            Stop();
        }
    }
}