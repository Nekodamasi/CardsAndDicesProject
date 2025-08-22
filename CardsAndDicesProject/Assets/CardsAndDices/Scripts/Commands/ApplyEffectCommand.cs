namespace CardsAndDices
{
    public class ApplyEffectCommand : ICommand
    {
        public  CompositeObjectId TargetObjectId { get; }
        public EffectData EffectData { get; }
        public EffectTargetType EffectTargetType { get; }
        public int Value { get; }

        public ApplyEffectCommand(CompositeObjectId targetObjectId, EffectData effectData, EffectTargetType effectTargetType, int value)
        {
            TargetObjectId = targetObjectId;
            EffectData = effectData;
            EffectTargetType = effectTargetType;
            Value = value;
        }
        public void Execute() { }
        public void Undo() { }
    }
}
