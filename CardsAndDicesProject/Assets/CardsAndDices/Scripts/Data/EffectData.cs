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
        [SerializeField] private  BuffDebuffType _buffDebuffType;
        public BuffDebuffType BuffDebuffType => _buffDebuffType;

        [Tooltip("更新タイミング")]
        [SerializeField] private TriggerTiming _updateTiming;
        public TriggerTiming UpdateTiming => _updateTiming;

        [Tooltip("持続ターン数または条件値")]
        [SerializeField] private int _durationValue;
        public int DurationValue => _durationValue;
    }
}
