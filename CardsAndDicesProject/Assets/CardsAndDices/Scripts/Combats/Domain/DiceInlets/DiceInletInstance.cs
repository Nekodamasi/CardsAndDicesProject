using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの状態を保持するデータ
    /// </summary>
    public class DiceInletInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private InletPackageProfile _inletPackageProfile;
        public int CurrentCountdownValue { get; private set; }
        public int CurrentUsageCount { get; private set; }
        public bool IsLock { get; private set; }

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// インスタンスが生きているか
        /// </summary>
        public bool IsAlive;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DiceInletInstance(CompositeObjectId compositeObjectId, InletPackageProfile inletPackageProfile)
        {
            _compositeObjectId = compositeObjectId;
            _inletPackageProfile = inletPackageProfile;
            ResetsageCount();
            ResetCountdown();
            IsLock = false;
            IsAlive = true;
        }

        /// <summary>
        /// 使用回数リセット
        /// </summary>
        public void ResetsageCount()
        {
            CurrentUsageCount = _inletPackageProfile.InletProfileId.InitialUsageCount;
        }

        /// <summary>
        /// カウントダウンリセット
        /// </summary>
        public void ResetCountdown()
        {
            CurrentCountdownValue = _inletPackageProfile.InletProfileId.InitialCountdownValue;
        }

        public bool ChkFaceAllowed(int faceValue)
        {
            if (IsLock) return false;
            if (CurrentUsageCount <= 0) return false;
            if (!IsAlive) return false;

            return _inletPackageProfile.ChkFaceAllowed(faceValue);
        }

        public bool AcceptableDice(int faceValue)
        {
            CurrentCountdownValue -= faceValue;
            if (CurrentCountdownValue <= 0)
            {
                CurrentCountdownValue = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// インレットのエフェクトタイプを取得します
        /// </summary>
        public InletEffectType InletEffectType => _inletPackageProfile.InletProfileId.InletEffectType;

        /// <summary>
        /// リセットタイミング
        /// </summary>
        public ActivationTiming UsageCountResetType => _inletPackageProfile.InletProfileId.UsageCountResetType;

        /// <summary>
        /// リセットタイミング
        /// </summary>
        public void SetIsLock(bool flg)
        {
            IsLock = flg;
        }


        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }

    }
}
