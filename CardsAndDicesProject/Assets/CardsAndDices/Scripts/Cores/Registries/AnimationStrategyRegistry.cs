using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// AnimationStrategyEntityとBaseAnimationStrategySOのマッピングを管理し、登録するためのScriptableObjectです。
    /// </summary>
    [CreateAssetMenu(fileName = "AnimationStrategyRegistry", menuName = "CardsAndDices/Core/Registries/AnimationStrategyRegistry")]
    public class AnimationStrategyRegistry : ScriptableObject
    {
        /// <summary>
        /// インスペクター上でマッピングを設定するための内部クラス
        /// </summary>
        [System.Serializable]
        private class AnimationStrategyMapping
        {
            public AnimationStrategyEntity AnimationEntity;
            public BaseAnimationStrategySO AnimationStrategy;
        }

        [SerializeField]
        private List<AnimationStrategyMapping> _strategyMappings = new();

        private Dictionary<AnimationStrategyEntity, BaseAnimationStrategySO> _registry;

        /// <summary>
        /// ScriptableObjectがロードされた際に呼び出され、インスペクターで設定されたリストを元に高速参照用の辞書を構築します。
        /// </summary>
        private void OnEnable()
        {
            // パフォーマンス向上のため、リストから辞書に変換
            if (_registry == null)
            { 
                _registry = new Dictionary<AnimationStrategyEntity, BaseAnimationStrategySO>();
                foreach (var mapping in _strategyMappings)
                {
                    if (mapping.AnimationEntity != null && !_registry.ContainsKey(mapping.AnimationEntity))
                    {
                        _registry.Add(mapping.AnimationEntity, mapping.AnimationStrategy);
                    }
                }
            }
        }

        /// <summary>
        /// 指定されたAnimationStrategyEntityに対応するBaseAnimationStrategySOを取得します。
        /// </summary>
        /// <param name="entity">取得したいアニメーション戦略のエンティティ。</param>
        /// <returns>対応するBaseAnimationStrategySO。見つからない場合はnullを返します。</returns>
        public BaseAnimationStrategySO GetStrategy(AnimationStrategyEntity entity)
        {
            if (entity == null) return null;
            
            _registry.TryGetValue(entity, out var strategy);
            return strategy;
        }
    }
}
