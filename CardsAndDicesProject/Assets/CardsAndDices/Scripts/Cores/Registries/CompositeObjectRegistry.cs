using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// シーン上の全てのBaseSpriteViewインスタンスを管理し、IDによる検索機能を提供するレジストリ。
    /// ScriptableObjectとして実装し、プロジェクト全体で単一のインスタンスを共有する。
    /// </summary>
    [CreateAssetMenu(fileName = "CompositeObjectRegistry", menuName = "CardsAndDices/Registries/CompositeObjectRegistry")]
    public class CompositeObjectRegistry : ScriptableObject
    {
        private readonly List<CompositeObjectId> _compositeObjects = new();

        [Inject]
        public void Initialize()
        {
            _compositeObjects.Clear();
        }

        /// <summary>
        /// CompositeObjectIdをレジストリに登録します。
        /// </summary>
        public void Register(CompositeObjectId compositeObjectId)
        {
            _compositeObjects.Add(compositeObjectId);
        }

        /// <summary>
        /// CompositeObjectIdをレジストリから登録解除します。
        /// </summary>
        public void Unregister(CompositeObjectId compositeObjectId)
        {
            _compositeObjects.Remove(compositeObjectId);
        }

        /// <summary>
        /// 登録されている全てのCompositeObjectIdを取得します。
        /// </summary>
        public IReadOnlyList<CompositeObjectId> GetAllCompositeObjectIds() => _compositeObjects;

        /// <summary>
        /// 指定されたタイプに一致する全てのCompositeObjectIdを取得します。
        /// </summary>
        /// <param name="typeEntity">検索するオブジェクトのタイプ。</param>
        /// <returns>指定されたタイプに一致するCompositeObjectIdのリスト。</returns>
        public List<CompositeObjectId> GetIdsByType(CompositeObjectIdTypeEntity typeEntity)
        {
            return _compositeObjects.Where(id => id.ObjectType == typeEntity).ToList();
        }
    }
}