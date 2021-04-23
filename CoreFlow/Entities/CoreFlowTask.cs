namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;

    public class CoreFlowTask : CoreFlowBaseEntity, ICoreFlowTask
    {
        public ICoreFlowEntity FlowCore { get; set; }
    }
}
