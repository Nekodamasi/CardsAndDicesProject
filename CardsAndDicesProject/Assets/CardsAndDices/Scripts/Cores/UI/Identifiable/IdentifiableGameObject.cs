using UnityEngine;
using VContainer;

namespace CardsAndDices
{
	/// <summary>
	/// CompositeObjectIdを持つMonoBehaviourクラス。
	/// インスペクターからObjectTypeを設定でき、自身のCompositeObjectIdを管理します。
	/// </summary>
	public class IdentifiableGameObject : MonoBehaviour, IGameInitializable
	{
        [Header("Components")]

		/// <summary>
		/// このオブジェクトのタイプを表す文字列。
		/// インスペクターから設定します。
		/// </summary>
		[SerializeField] private CompositeObjectIdTypeEntity _objectType;
		[SerializeField] private IdentifiableGameObject _Owner;
	
		/// <summary>
		/// このオブジェクトのタイプを表す文字列。
		/// インスペクターから設定します。
		/// </summary>
		[SerializeField] private string _displayCompositeObjectId;

		private CompositeObjectIdManager _idManager;

		/// <summary>
		/// このMonoBehaviourに割り当てられたCompositeObjectId。
		/// </summary>
		public CompositeObjectId CompositeObjectId { get; private set; }

		[Inject]
		public void Construct(CompositeObjectIdManager idManager)
		{
			_idManager = idManager;
        }

		/// <summary>
		/// コンポーネントの初期化を行います。
		/// </summary>
		public void OnAwake()
		{
			CompositeObjectId = _idManager.CreateId(_objectType);
			_displayCompositeObjectId = CompositeObjectId.ToString();
		}

		public void OnStart()
		{
			if (_Owner is null) return;
			CompositeObjectId.Owner = _Owner.CompositeObjectId;
		}

		/// <summary>
		/// このオブジェクトのオーナー（親）を設定します。
		/// </summary>
		/// <param name="ownerId">親となるCompositeObjectId。</param>
		public void SetOwner(CompositeObjectId ownerId)
		{
			if (CompositeObjectId != null)
			{
				CompositeObjectId.Owner = ownerId;
				Debug.Log($"Set owner for {gameObject.name} ({CompositeObjectId.UniqueId}) to {ownerId.UniqueId}");
			}
			else
			{
				Debug.LogWarning($"Attempted to set owner on {gameObject.name} before ObjectId was initialized.");
			}
		}
	}
}