namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Enums;
    using CoreFlow.Model.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CoreFlowEntity : CoreFlowBaseEntity, ICoreFlowEntity
    {
        public DateTime? DateStarted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public CoreFlowStatus Status { get; set; }

        public IList<CoreFlowTask> Tasks { get; private set; }
        public IList<CoreFlowTransition> Transitions { get; private set; }

        public Action OnActivated { get; set; }
        public Action OnCompleted { get; set; }

        public CoreFlowEntity()
        {
            Tasks = new List<CoreFlowTask>();
            Transitions = new List<CoreFlowTransition>();
        }

        public void AddTask(CoreFlowTask task)
        {
            if (string.IsNullOrEmpty(task.Name))
                throw new Exception("Task Name is required.");

            if(task.TaskType == CoreFlowTaskType.Start && Tasks.Any(tsk => tsk.TaskType == CoreFlowTaskType.Start))
                throw new InvalidOperationException("There is already a Start Task in the flow.");

            Tasks.Add(task);
        }

        public void AddTransition(CoreFlowTransition transition)
        {
            if (transition == null)
                throw new NullReferenceException("Cannot add null transition.");
            
            if (transition.FromTask == null)
                throw new NullReferenceException("Cannot add transition with null FromTask.");

            if (transition.ToTask == null)
                throw new NullReferenceException("Cannot add null transition with null ToTask.");

            if (Transitions.Any(tr => tr.FromTask == transition.FromTask && tr.ToTask == transition.ToTask))
                throw new InvalidOperationException("Transition with same FromTask and ToTask already exists.");

            if (string.IsNullOrEmpty(transition.Name))
                throw new Exception("Transition Name is required.");

            Transitions.Add(transition);
        }
    }
}
