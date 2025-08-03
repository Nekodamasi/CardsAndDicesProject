using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer; // VContainer.Inject を使用するために追加

namespace CardsAndDice
{
    /// <summary>
    /// SpriteUIに関連するイベントの登録、配信、解除を一元管理する中央ハブ。
    /// ScriptableObjectを使用したシングルトンとして実装されています。
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteCommandBus", menuName = "CardsAndDice/Systems/SpriteCommandBus")]
    public class SpriteCommandBus : ScriptableObject
    {
        private readonly Dictionary<Type, List<Action<ICommand>>> _subscribers = new();

        /// <summary>
        /// ScriptableObjectが初期化される時の処理。
        /// VContainerによって呼び出されます。
        /// </summary>
        [Inject]
        public void Initialize()
        {
            // 購読者リストをクリア
            _subscribers.Clear();
        }

        /// <summary>
        /// コマンドタイプに対する購読者を登録します。
        /// </summary>
        /// <typeparam name="T">購読するコマンドの型</typeparam>
        /// <param name="handler">コマンドを処理するハンドラー</param>
        public void On<T>(Action<T> handler) where T : ICommand
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Action<ICommand>>();
            }

            _subscribers[type].Add((command) => handler((T)command));
        }

        /// <summary>
        /// コマンドタイプに対する購読を解除します。
        /// </summary>
        /// <typeparam name="T">購読解除するコマンドの型</typeparam>
        /// <param name="handler">解除するハンドラー</param>
        public void Off<T>(Action<T> handler) where T : ICommand
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var index = _subscribers[type].FindIndex(
                action => action.Target == handler.Target && 
                action.Method == handler.Method
            );

            if (index >= 0)
            {
                _subscribers[type].RemoveAt(index);
            }
        }

        /// <summary>
        /// コマンドを発行し、登録された購読者に配信します。
        /// </summary>
        /// <param name="command">発行するコマンド</param>
        public void Emit(ICommand command)
        {
            var type = command.GetType();
//            Debug.Log($"[SpriteCommandBus] Emit: Command {type.Name} received.");

            if (!_subscribers.ContainsKey(type))
            {
//                Debug.LogWarning($"[SpriteCommandBus] Emit: No subscribers found for command {type.Name}.");
                return;
            }

//            Debug.Log($"[SpriteCommandBus] Emit: Found {_subscribers[type].Count} subscribers for {type.Name}.");
            foreach (var handler in _subscribers[type])
            {
                try
                {
//                    Debug.Log($"[SpriteCommandBus] Emit: Invoking handler for {type.Name} (Target: {handler.Target?.GetType().Name ?? "None"}, Method: {handler.Method.Name}).");
                    handler(command);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[SpriteCommandBus] Error handling command {type.Name}: {e}");
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