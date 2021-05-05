namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;
    using System;

    public class CoreFlowTransition : CoreFlowBaseEntity, ICoreFlowTransition
    {
        public ICoreFlowTask FromTask { get; set; }
        public ICoreFlowTask ToTask { get; set; }
        public ICoreFlowEntity FlowCore { get; set; }

        public DateTime? DateActivated { get; set; }
        public DateTime? DateCompleted { get; set; }

        public string Condition { get; set; }
        public bool Completed { get; set; }

        public CoreFlowTransition() {
        }

        public CoreFlowTransition(string name, ICoreFlowTask fromTask, ICoreFlowTask toTask)
        {
            Name = name;
            FromTask = fromTask;
            ToTask = toTask;
        }
    }
}
