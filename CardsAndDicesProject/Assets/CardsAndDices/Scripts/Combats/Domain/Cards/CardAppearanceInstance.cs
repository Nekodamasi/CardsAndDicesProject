using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// カードの見た目を管理するインスタンス。
    /// </summary>
    public class CardAppearanceInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private AppearanceProfile _appearanceProfile;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CardAppearanceInstance(CompositeObjectId compositeObjectId, AppearanceProfile appearanceProfile)
        {
            _compositeObjectId = compositeObjectId;
            _appearanceProfile = appearanceProfile;
        }

        /// <summary>
        /// クリーチャーカードを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// クリーチャーカードを一意に識別するID。
        /// </summary>
        public AppearanceProfile AppearanceProfile => _appearanceProfile;

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }
    }
}
