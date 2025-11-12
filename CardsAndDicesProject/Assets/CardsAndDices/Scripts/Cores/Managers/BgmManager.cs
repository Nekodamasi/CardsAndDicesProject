using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace CardsAndDices
{
    /// <summary>
    /// VFXの再生とオブジェクトプールを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "BgmManager", menuName = "CardsAndDices/Cores/Managers/BgmManager")]
    public class BgmManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        public AudioMixer audioMixer;
        public AudioMixerGroup bgmMixerGroup;

        public AudioMixerSnapshot normalMixerSnapshot;
        public AudioMixerSnapshot fadeOutMixerSnapshot;
        public AudioMixerSnapshot fadeInMixerSnapshot;

        private GameEventBus _eventBus;
        private AudioSource _audioSource;
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        public void Initialize(GameEventBus eventBus, AudioSource audioSource)
        {
            ClearCollections();

            _eventBus = eventBus;
            _audioSource = audioSource;
            _cancellationTokenSource = new CancellationTokenSource();
            _eventBus.On<PlayBGMEvent>(PlayBGMStart);
        }

        private void ClearCollections()
        {
            LoadVolume();
        }

        public void Dispose()
        {
            _eventBus.Off<PlayBGMEvent>(PlayBGMStart);
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private void PlayBGMStart(PlayBGMEvent evt)
        {
            PlayBGM(evt.AudioClip);
        }

        public void PlayBGM(AudioClip clip, float fadeDuration = 1.0f)
        {
            SwitchBGM(clip, fadeDuration).Forget();
        }

        private async UniTask SwitchBGM(AudioClip clip, float fadeDuration)
        {
            var token = _cancellationTokenSource.Token;
            fadeOutMixerSnapshot.TransitionTo(fadeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration), cancellationToken: token);

            if (token.IsCancellationRequested) return;

            // BGM切替
            _audioSource.clip = clip;
            _audioSource.Play();

            // フェードイン
            fadeInMixerSnapshot.TransitionTo(fadeDuration);
        }

        public void SetVolume(float volume)
        {
            //
            float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
            audioMixer.SetFloat("BGMvolume", dB);
            PlayerPrefs.SetFloat("BGMvolume", volume);
        }

        public void LoadVolume()
        {
            float savedVolume = PlayerPrefs.GetFloat("BGMvolume", 1.0f);
            SetVolume(savedVolume);
        }
    }
}
