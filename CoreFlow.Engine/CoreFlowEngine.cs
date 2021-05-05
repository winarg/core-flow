
namespace CoreFlow.Engine
{
    using System;
    using CoreFlow.Model.Entities;
    using CoreFlow.Model.Enums;
    using CoreFlow.Model.Exceptions;
    using CoreFlow.Model.Interfaces;

    public class CoreFlowEngine
    {
        private EngineHelper _engineHelper = new EngineHelper();
        public CoreFlowEngine()
        {
        }

        public bool StartFlow(ICoreFlowEntity flow)
        {
            CheckIfNull(flow);

            if (flow.Status != Model.Enums.CoreFlowStatus.NotStarted)
                throw new CoreFlowException("Only NotStarted flow can be started!");

            var startTask = _engineHelper.GetEntryTask(flow);
            var startTaskTransitions = _engineHelper.GetTaskTransitions(flow, startTask);

            foreach (var transition in startTaskTransitions)
            {
                ActivateTask(flow, transition.ToTask);
            }

            return true;
        }

        public bool ActivateTask(ICoreFlowEntity flow, ICoreFlowTask task)
        {
            CheckIfNull(flow);
            CheckIfNull(task);

            if (task.Status != CoreFlowTaskStatus.NotStarted)
                throw new CoreFlowException("Only a not started task can be activated.");

            if (task.TaskType == CoreFlowTaskType.End) {
                if (!_engineHelper.CanFlowBeCompleted(flow))
                    return true;

                task.Status = CoreFlowTaskStatus.Completed;
                task.DateCompleted = DateTime.UtcNow;

                flow.Status = CoreFlowStatus.Completed;
                flow.DateCompleted = DateTime.UtcNow;
                return true;
            }

            task.Status = CoreFlowTaskStatus.Active;
            task.DateActivated = DateTime.UtcNow;

            return true;
        }

        public bool CompleteTask(ICoreFlowEntity flow, ICoreFlowTask task, int? transitionId = null)
        {
            CheckIfNull(flow);
            CheckIfNull(task);

            if (task.Status != CoreFlowTaskStatus.Active)
                throw new CoreFlowException("Only an task can be completed.");

            task.Status = CoreFlowTaskStatus.Completed;
            task.DateCompleted = DateTime.UtcNow;

            if (transitionId == null)
            {
                // Activate all transitions
                var allTaskTransitions = _engineHelper.GetTaskTransitions(flow, task);
                foreach (var transition in allTaskTransitions)
                {
                    ActivateTask(flow, transition.ToTask);
                }
            }
            else
            {
                // Activate the selected transition
                var selectedTransition = _engineHelper.GetTransition(flow, task, transitionId.Value);
                ActivateTask(flow, selectedTransition.ToTask);

            }

            return true;
        }

        // Check if object is null and throw an exception
        private void CheckIfNull<T>(T entity)
        {
            if(entity == null)
                throw new NullReferenceException();
        }

    }
}
