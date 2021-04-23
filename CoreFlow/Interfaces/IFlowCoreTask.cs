namespace CoreFlow.Model.Interfaces
{
    public interface IFlowCoreTask : IFlowCoreBaseEntity
    {
        IFlowCoreEntity FlowCore { get; set; }
    }
}
