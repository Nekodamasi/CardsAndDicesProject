using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 現在アクティブなインレットの能力プロファイルを一元管理するレジストリ。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletAbilityRegistry", menuName = "CardsAndDices/Managers/DiceInletAbilityRegistry")]
    public class DiceInletAbilityRegistry : ScriptableObject
    {
        private readonly Dictionary<CompositeObjectId, InletAbilityProfile> _activeProfiles = new();

        /// <summary>
        /// 指定したインレットに能力プロファイルを登録します。
        /// </summary>
        /// <param name="inletId">インレットのID。</param>
        /// <param name="profile">登録する能力プロファイル。</param>
        public void Register(CompositeObjectId inletId, InletAbilityProfile profile)
        {
            _activeProfiles[inletId] = profile;
        }

        /// <summary>
        /// 指定したインレットの能力プロファイルの登録を解除します。
        /// </summary>
        /// <param name="inletId">インレットのID。</param>
        public void Unregister(CompositeObjectId inletId)
        { 
            _activeProfiles.Remove(inletId);
        }

        /// <summary>
        /// 指定したインレットの現在の能力プロファイルを取得します。
        /// </summary>
        /// <param name="inletId">インレットのID。</param>
        /// <returns>対応する能力プロファイル。登録されていない場合はnull。</returns>
        public InletAbilityProfile GetProfile(CompositeObjectId inletId)
        {
            _activeProfiles.TryGetValue(inletId, out var profile);
            return profile;
        }

        /// <summary>
        /// レジストリをクリアします。
        /// </summary>
        public void Clear()
        {
            _activeProfiles.Clear();
        }
    }
}