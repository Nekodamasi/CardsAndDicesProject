using System;

namespace CardsAndDices
{
	/// <summary>
	/// ゲームオブジェクトを一意に識別するための複合IDクラス。
	/// ユニークID、オブジェクトタイプ、および階層構造を表すオーナー情報を持つ。
	/// </summary>
	[Serializable] // Unity Inspectorで表示可能にするため
	public class CompositeObjectId : IEquatable<CompositeObjectId>
	{
		/// <summary>
		/// このオブジェクトのユニークな識別子（シーケンス番号）。
		/// </summary>
		public long UniqueId { get; private set; }

		/// <summary>
		/// このオブジェクトのタイプを表す文字列（例: "Card", "Dice", "Button"）。
		/// </summary>
		public string ObjectType { get; private set; }

		/// <summary>
		/// このオブジェクトの親となるCompositeObjectId。
		/// 階層のルートオブジェクトの場合はnull。
		/// </summary>
		public CompositeObjectId Owner { get; set; }

		/// <summary>
		/// CompositeObjectIdの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="uniqueId">このオブジェクトのユニークID。</param>
		/// <param name="objectType">このオブジェクトのタイプ。</param>
		/// <param name="owner">このオブジェクトのオーナーとなるCompositeObjectId。ルートの場合はnull。</param>
		public CompositeObjectId(long uniqueId, string objectType, CompositeObjectId owner)
		{
			UniqueId = uniqueId;
			ObjectType = objectType;
			Owner = owner;
		}

		/// <summary>
		/// 2つのCompositeObjectIdが等しいかどうかを判断します。
		/// UniqueIdとObjectTypeが一致する場合に等しいとみなします。
		/// </summary>
		/// <param name="other">比較するCompositeObjectId。</param>
		/// <returns>等しい場合はtrue、それ以外はfalse。</returns>
		public bool Equals(CompositeObjectId other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return UniqueId == other.UniqueId && ObjectType == other.ObjectType;
		}

		/// <summary>
		/// このインスタンスのハッシュコードを返します。
		/// </summary>
		/// <returns>このインスタンスのハッシュコード。</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (UniqueId.GetHashCode() * 397) ^ (ObjectType != null ? ObjectType.GetHashCode() : 0);
			}
		}

		/// <summary>
		/// 2つのCompositeObjectIdが等しいかどうかを判断します。
		/// </summary>
		/// <param name="obj">比較するオブジェクト。</param>
		/// <returns>等しい場合はtrue、それ以外はfalse。</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((CompositeObjectId)obj);
		}

		/// <summary>
		/// CompositeObjectIdの文字列表現を返します。
		/// </summary>
		/// <returns>CompositeObjectIdの文字列表現。</returns>
		public override string ToString()
		{
			return $"[ID:{UniqueId}, Type:{ObjectType}, Owner:{(Owner != null ? Owner.UniqueId.ToString() : "None")}]";
		}
	}
}