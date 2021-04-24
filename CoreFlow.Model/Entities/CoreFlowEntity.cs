namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Enums;
    using CoreFlow.Model.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CoreFlowEntity : CoreFlowBaseEntity, ICoreFlowEntity
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        public CoreFlowStatus Status { get; set; }

        public IList<ICoreFlowTask> Tasks { get; set; }
        public IList<ICoreFlowTransition> Transitions { get; set; }
    }
}
