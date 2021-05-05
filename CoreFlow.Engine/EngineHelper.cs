namespace CoreFlow.Engine
{
    using System;
    using System.Linq;
    using CoreFlow.Model.Entities;
    using CoreFlow.Model.Exceptions;
    using CoreFlow.Model.Interfaces;
    using CoreFlow.Model.Enums;
    using System.Collections.Generic;

    public class EngineHelper
    {
        public ICoreFlowTask GetEntryTask(ICoreFlowEntity flow)
        {
            if (flow == null)
                throw new NullReferenceException();

            if (!flow.Tasks.Any())
                throw new CoreFlowException("Flow does not contain any Tasks!");

            return flow.Tasks.SingleOrDefault(c => c.TaskType == CoreFlowTaskType.Start);
        }

        public IList<CoreFlowTransition> GetTaskTransitions(ICoreFlowEntity flow, ICoreFlowTask task, bool completed = false)
        {
            return flow.Transitions.Where(c => c.FromTask == task && !completed).ToList();
        }

        public ICoreFlowTransition GetTransition(ICoreFlowEntity flow, ICoreFlowTask task, int transitionId)
        {
            return flow.Transitions.FirstOrDefault(c => c.Id == transitionId && c.FromTask == task);
        }

        public IList<CoreFlowTask> GetActiveTasks(ICoreFlowEntity flow)
        {
            return flow.Tasks.Where(c => c.Status == CoreFlowTaskStatus.Active).ToList();
        }

        public bool CanFlowBeCompleted(ICoreFlowEntity flow)
        {
            return (flow.Status == CoreFlowStatus.Active && GetActiveTasks(flow).Count == 0);
        }
    }
}
