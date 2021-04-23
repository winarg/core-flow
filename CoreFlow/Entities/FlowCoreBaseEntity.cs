namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public class FlowCoreBaseEntity : IFlowCoreBaseEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }
}
