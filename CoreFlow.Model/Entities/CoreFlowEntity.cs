namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Enums;
    using CoreFlow.Model.Interfaces;
    using System;
    using System.Collections.Generic;

    public class CoreFlowEntity : CoreFlowBaseEntity, ICoreFlowEntity
    {
        public DateTime? DateStarted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public CoreFlowStatus Status { get; set; }

        public IList<CoreFlowTask> Tasks { get; set; }
        public IList<CoreFlowTransition> Transitions { get; set; }

        public Action OnActivated { get; set; }
        public Action OnCompleted { get; set; }

        public CoreFlowEntity()
        {
            Tasks = new List<CoreFlowTask>();
            Transitions = new List<CoreFlowTransition>();
        }
    }
}
