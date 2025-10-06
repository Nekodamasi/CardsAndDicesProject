using System;

namespace CardsAndDices 
{
    [Serializable]
     /// <summary>
    /// エネミーグループの配置場所。エネミーグループの１体が指定した配置場所に配置される
    /// </summary>
   public class EnemyPlacement
    {
        public EnemyGroup EnemyGroup;
        public LinePosition Position;
        public SlotLocation Location;
    }
}
