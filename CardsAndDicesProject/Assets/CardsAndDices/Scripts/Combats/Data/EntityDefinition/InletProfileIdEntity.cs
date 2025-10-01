using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// インレットを情報を一意に扱うプロフィール
    /// </summary>
    [CreateAssetMenu(fileName = "InletPro_", menuName = "CardsAndDices/Combats/Data/EntityDefinition/InletProfileIdEntity")]
    public class InletProfileIdEntity : BaseEntityDefinition
    {
        [Tooltip("インレット発動ビュータイプ")]
        [SerializeField] private InletActivationViewType _inletActivationViewType;
        public InletActivationViewType InletActivationViewType => _inletActivationViewType;

        [Tooltip("投入可能なダイスの目")]
        [SerializeField] private AllowedDiceFacesEntity _allowedDiceFaces;
        public AllowedDiceFacesEntity AllowedDiceFacesEntity => _allowedDiceFaces;

        [Tooltip("インレットの初期カウントダウン値")]
        [SerializeField] private int _initialCountdownValue = 1;
        public int InitialCountdownValue => _initialCountdownValue;

        [Tooltip("インレットの初期使用可能回数")]
        [SerializeField] private int _initialUsageCount = 1;
        public int InitialUsageCount => _initialUsageCount;

        [Tooltip("使用可能回数がリセットされるタイミング")]
        [SerializeField] private ActivationTiming _resetTiming;
        public ActivationTiming UsageCountResetType => _resetTiming;

        [Tooltip("インレット発動時の効果")]
        [SerializeField] private InletEffectType _inletEffectType;
        public InletEffectType InletEffectType => _inletEffectType;

        [Tooltip("インレットで発動またはロックされるアビリティ")]
        [SerializeField] private List<AbilityDataEntity> _abilities = new List<AbilityDataEntity>();
    }
}
