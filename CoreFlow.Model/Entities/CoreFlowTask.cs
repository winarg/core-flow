namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Enums;
    using CoreFlow.Model.Interfaces;
    using System;

    public class CoreFlowTask : CoreFlowBaseEntity, ICoreFlowTask
    {
        public ICoreFlowEntity FlowCore { get; set; }

        public DateTime? DateActivated { get; set; }
        public DateTime? DateCompleted { get; set; }

        public CoreFlowTaskType TaskType { get; set; }
        public CoreFlowTaskStatus Status { get; set; }

        public Action<int> OnActivated { get; set; }
        public Action<int> OnCompleted { get; set; }

        public CoreFlowTask() {
        }

        public CoreFlowTask(string name, CoreFlowTaskType type)
        {
            Name = name;
            TaskType = type;
        }
    }
}
