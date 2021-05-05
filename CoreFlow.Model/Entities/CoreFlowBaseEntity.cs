namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public abstract class CoreFlowBaseEntity : ICoreFlowBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
