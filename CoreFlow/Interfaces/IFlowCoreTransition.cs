namespace CoreFlow.Model.Interfaces
{
    public interface IFlowCoreTransition : IFlowCoreBaseEntity
    {
        IFlowCoreTask FromTask { get; set; }
        IFlowCoreTask ToTask { get; set; }
        IFlowCoreEntity FlowCore { get; set; }
    }
}
