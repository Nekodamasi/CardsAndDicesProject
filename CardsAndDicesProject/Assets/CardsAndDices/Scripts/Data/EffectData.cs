using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// バフ／デバフ効果の内容を定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "EffectData", menuName = "CardsAndDices/EffectData")]
    public class EffectData : ScriptableObject
    {
        [Tooltip("効果を識別するための一意なID")]
        [SerializeField] private CompositeObjectId _effectId;
        public CompositeObjectId EffectId => _effectId;

        [Tooltip("効果名")]
        [SerializeField] private new string name;
        public string Name => name;

        [Tooltip("ステータス増減量")]
        [SerializeField] private int _value;
        public int Value => _value;

        [Tooltip("適用対象タイプ")]
        [SerializeField] private EffectTargetType _targetType;
        public EffectTargetType TargetType => _targetType;

        [Tooltip("持続条件タイプ")]
        [SerializeField] private EffectDurationType _durationType;
        public EffectDurationType DurationType => _durationType;

        [Tooltip("持続ターン数または条件値")]
        [SerializeField] private int _durationValue;
        public int DurationValue => _durationValue;

        [Tooltip("効果終了のトリガーとなるイベント名の一覧")]
        [SerializeField] private List<string> _eventTriggers = new List<string>();
        public List<string> EventTriggers => _eventTriggers;
    }
}
