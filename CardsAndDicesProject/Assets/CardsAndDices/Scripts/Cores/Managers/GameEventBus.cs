using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// ゲーム内イベントの登録、配信、解除を一元管理する中央ハブ。
    /// ScriptableObjectを使用したシングルトンとして実装されています。
    /// </summary>
    [CreateAssetMenu(fileName = "GameEventBus", menuName = "CardsAndDices/Core/Identifiable/GameEventBus")]
    public class GameEventBus : ScriptableObject
    {
        private readonly Dictionary<Type, Delegate> _subscribers = new();

        /// <summary>
        /// ScriptableObjectが初期化される時の処理。
        /// VContainerによって呼び出されます。
        /// </summary>
        [Inject]
        public void Initialize()
        {
            _subscribers.Clear();
        }

        /// <summary>
        /// イベントタイプに対する購読者を登録します。
        /// </summary>
        /// <typeparam name="T">購読するイベントの型</typeparam>
        /// <param name="handler">イベントを処理するハンドラー</param>
        public void On<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var existingHandler))
            {
                _subscribers[type] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                _subscribers[type] = handler;
            }
        }

        /// <summary>
        /// イベントタイプに対する購読を解除します。
        /// </summary>
        /// <typeparam name="T">購読解除するイベントの型</typeparam>
        /// <param name="handler">解除するハンドラー</param>
        public void Off<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var existingHandler))
            {
                var newHandler = Delegate.Remove(existingHandler, handler);
                if (newHandler == null)
                {
                    _subscribers.Remove(type);
                }
                else
                {
                    _subscribers[type] = newHandler;
                }
            }
        }

        /// <summary>
        /// イベントを発行し、登録された購読者に配信します。
        /// </summary>
        /// <param name="eventInstance">発行するイベント</param>
        public void Emit(IEvent eventInstance)
        {
            var type = eventInstance.GetType();
            if (_subscribers.TryGetValue(type, out var handler))
            {
                try
                {
                    // 登録されたデリゲートを実行
                    handler.DynamicInvoke(eventInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[GameEventBus] Error handling event {type.Name}: {e}");
                }
            }
        }

        /// <summary>
        /// すべての購読を解除します。
        /// シーン遷移時などに呼び出してください。
        /// </summary>
        public void ClearAllSubscriptions()
        {
            _subscribers.Clear();
        }
    }
}