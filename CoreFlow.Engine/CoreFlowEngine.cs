
namespace CoreFlow.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            _engineHelper.IsFlowValid(flow);

            if (flow.Status != Model.Enums.CoreFlowStatus.NotStarted)
                throw new CoreFlowException("Only NotStarted flow can be started!");

            flow.Status = CoreFlowStatus.Active;
            flow.DateStarted = DateTime.UtcNow;

            if (flow.OnActivated != null)
                flow.OnActivated.Invoke();

            var startTask = _engineHelper.GetEntryTask(flow);

            ActivateTask(flow, startTask);
            CompleteTask(flow, startTask);

            return true;
        }

        public bool ActivateTask(ICoreFlowEntity flow, ICoreFlowTask task)
        {
            CheckIfNull(flow);
            CheckIfNull(task);

            if (task.Status != CoreFlowTaskStatus.NotStarted)
                throw new CoreFlowException("Only a not started task can be activated.");

            if (task.OnActivated != null)
                task.OnActivated.Invoke();

            if (!_engineHelper.CanTaskBeActivated(flow, task))
                return false;

            if (task.TaskType == CoreFlowTaskType.End) {
                if (!_engineHelper.CanFlowBeCompleted(flow))
                    return true;

                task.Status = CoreFlowTaskStatus.Completed;
                task.DateCompleted = DateTime.UtcNow;

                flow.Status = CoreFlowStatus.Completed;
                flow.DateCompleted = DateTime.UtcNow;

                if (flow.OnCompleted != null)
                    flow.OnCompleted.Invoke();

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
                throw new CoreFlowException("Only an active task can be completed.");

            if (task.OnCompleted != null)
                task.OnCompleted.Invoke();

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

        public IList<CoreFlowTask> GetActiveTasks(ICoreFlowEntity flow)
        {
            return flow.Tasks.Where(task => task.Status == CoreFlowTaskStatus.Active).ToList();
        }

        // Check if object is null and throw an exception
        private void CheckIfNull<T>(T entity)
        {
            if(entity == null)
                throw new NullReferenceException();
        }

    }
}
