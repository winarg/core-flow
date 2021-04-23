namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public class CoreFlowTransition : CoreFlowBaseEntity, ICoreFlowTransition
    {
        public ICoreFlowTask FromTask { get; set; }
        public ICoreFlowTask ToTask { get; set; }
        public ICoreFlowEntity FlowCore { get; set; }
    }
}
