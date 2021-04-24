namespace CoreFlow.Model.Interfaces
{
    using System;

    public interface ICoreFlowTransition : ICoreFlowBaseEntity
    {
        DateTime? DateActivated { get; set; }
        DateTime? DateCompleted { get; set; }

        bool Completed { get; set; }

        string Condition { get; set; }

        ICoreFlowTask FromTask { get; set; }
        ICoreFlowTask ToTask { get; set; }
        ICoreFlowEntity FlowCore { get; set; }
    }
}
