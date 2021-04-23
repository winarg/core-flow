namespace CoreFlow.Model.Interfaces
{
    public interface ICoreFlowTransition : ICoreFlowBaseEntity
    {
        ICoreFlowTask FromTask { get; set; }
        ICoreFlowTask ToTask { get; set; }
        ICoreFlowEntity FlowCore { get; set; }
    }
}
