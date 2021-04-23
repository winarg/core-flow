namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public class FlowCoreTask : FlowCoreBaseEntity, IFlowCoreTask
    {
        public IFlowCoreEntity FlowCore { get; set; }
    }
}
