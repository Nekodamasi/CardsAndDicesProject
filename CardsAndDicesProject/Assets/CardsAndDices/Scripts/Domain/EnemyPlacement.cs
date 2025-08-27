using System;

namespace CardsAndDices 
{
    [Serializable]
    public class EnemyPlacement
    {
        public EnemyGroup EnemyGroup;
        public LinePosition Position;
        public SlotLocation Location;
    }
}
