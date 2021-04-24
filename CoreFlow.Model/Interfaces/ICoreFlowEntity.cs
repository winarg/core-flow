namespace CoreFlow.Model.Interfaces
{
    using CoreFlow.Model.Enums;
    using System;
    using System.Collections.Generic;

    public interface ICoreFlowEntity : ICoreFlowBaseEntity
    {
        DateTime DateCreated { get; set; }
        DateTime DateCompleted { get; set; }

        CoreFlowStatus Status { get; set; }

        IList<ICoreFlowTask> Tasks { get; set; }
        IList<ICoreFlowTransition> Transitions { get; set; }
    }
}
