﻿namespace CoreFlow.Model.Interfaces
{
    using CoreFlow.Model.Enums;
    using System;

    public interface ICoreFlowTask : ICoreFlowBaseEntity
    {
        DateTime? DateActivated { get; set; }
        DateTime? DateCompleted { get; set; }

        CoreFlowTaskType TaskType { get; set; }
        CoreFlowTaskStatus Status { get; set; }

        ICoreFlowEntity FlowCore { get; set; }

        Action<int> OnActivated { get; set; }
        Action<int> OnCompleted { get; set; }
    }
}
