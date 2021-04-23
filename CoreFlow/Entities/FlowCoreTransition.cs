namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public class FlowCoreTransition : FlowCoreBaseEntity, IFlowCoreTransition
    {
        public IFlowCoreTask FromTask { get; set; }
        public IFlowCoreTask ToTask { get; set; }
        public IFlowCoreEntity FlowCore { get; set; }
    }
}
