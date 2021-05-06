namespace CoreFlow.Model.Interfaces
{
    using CoreFlow.Model.Entities;
    using CoreFlow.Model.Enums;
    using System;
    using System.Collections.Generic;

    public interface ICoreFlowEntity : ICoreFlowBaseEntity
    {
        DateTime? DateStarted { get; set; }
        DateTime? DateCompleted { get; set; }

        CoreFlowStatus Status { get; set; }

        IList<CoreFlowTask> Tasks { get; }
        IList<CoreFlowTransition> Transitions { get; }

        Action OnActivated { get; set; }
        Action OnCompleted { get; set; }
    }
}
