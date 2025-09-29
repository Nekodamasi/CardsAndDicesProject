using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードのアイコン１つを管理するインスタンス。
    /// </summary>
    public class IconStatusInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private SharedIconElementTypeEntity _iconTypeEntity;
        private SharedIconElementStatus _sharedIconElementStatus;
        private int _displayiconValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IconStatusInstance(CompositeObjectId compositeObjectId, SharedIconElementTypeEntity iconTypeEntity, SharedIconElementStatus sharedIconElementStatus)
        {
            _compositeObjectId = compositeObjectId;
            _iconTypeEntity = iconTypeEntity;
            _sharedIconElementStatus = sharedIconElementStatus;
        }

        /// <summary>
        /// クリーチャーステータスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// アイコンが管理している数値のタイプ。
        /// </summary>
        public SharedIconElementTypeEntity SharedIconElementTypeEntity => _iconTypeEntity;

        /// <summary>
        /// アイコンが管理している数値のタイプ。
        /// </summary>
        public SharedIconElementStatus CurrentStatus => _sharedIconElementStatus;

        /// <summary>
        /// 現在表示しているアイコンの値。
        /// </summary>
        public int DisplayiconValue => _displayiconValue;

        /// <summary>
        /// 表示する値を更新します
        /// </summary>
        public void SetIconValue(int value)
        {
            _displayiconValue = value;
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }
    }
}
