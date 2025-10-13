namespace CardsAndDices
{
    /// <summary>
    /// 攻撃を受けたクリーチャーのリアクション演出を行うイベント
    /// </summary>
    public class CreatureAttackEvent : IEvent
    {
        public CreatureAttackContext CreateAttackContext { get; }
        public CreatureAttackEvent(CreatureAttackContext createAttackContext)
        {
            CreateAttackContext = createAttackContext;
        }
    }
}
