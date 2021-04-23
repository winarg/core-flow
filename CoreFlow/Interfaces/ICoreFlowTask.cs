namespace CoreFlow.Model.Interfaces
{
    public interface ICoreFlowTask : ICoreFlowBaseEntity
    {
        ICoreFlowEntity FlowCore { get; set; }
    }
}
