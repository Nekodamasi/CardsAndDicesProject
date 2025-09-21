namespace CardsAndDices
{
    /// <summary>
    /// ゲーム全体で使用される描画のソート順（Sorting Order）を一元管理します。
    /// </summary>
    public static class SortingOrders
    {
        /// <summary>
        /// ワールド空間に配置されるオブジェクト（カードスロットなど）のソート順。
        /// </summary>
        public static class World
        {
            public const int CardSlot = 10;
            public const int Line = 5;
        }

        /// <summary>
        /// カード自体の状態に応じたソート順。
        /// </summary>
        public static class Cards
        {
            public const int Default = 20;
            public const int Hovered = 150;
            public const int Dragging = 200;
        }

        /// <summary>
        /// カード自体の状態に応じたソート順。
        /// </summary>
        public static class Dices
        {
            public const int Default = 20;
            public const int Hovered = 150;
            public const int Dragging = 200;
        }

        /// <summary>
        /// UIやエフェクトなど、最前面に表示される要素のソート順。
        /// </summary>
        public static class Overlays
        {
            public const int Effects = 300;
            public const int DamageText = 310;
            public const int UI = 1000;
        }
    }
}
