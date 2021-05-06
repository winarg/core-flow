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

        public bool CanTaskBeActivated(ICoreFlowEntity flow, ICoreFlowTask task)
        {
            var activeTasks = GetActiveTasks(flow);
            if(!activeTasks.Any())
                return true;

            foreach (var currentActiveTask in activeTasks)
            {
                if (CanGoToTask(flow, currentActiveTask, task))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns if there is a way to reach one task from another
        /// </summary>
        /// <param name="flow"></param>
        /// <param name="fromTask"></param>
        /// <param name="toTask"></param>
        /// <returns></returns>
        public bool CanGoToTask(ICoreFlowEntity flow, ICoreFlowTask fromTask, ICoreFlowTask toTask)
        {
            return GetPathsFromTaskToTask(flow, fromTask, toTask).Any();
        }

        /// <summary>
        /// Returns a list with all possible paths from a task to another
        /// </summary>
        /// <param name="flow"></param>
        /// <param name="fromTask"></param>
        /// <param name="toTask"></param>
        /// <returns></returns>
        public List<ICoreFlowTransition> GetPathsFromTaskToTask(ICoreFlowEntity flow, ICoreFlowTask fromTask, ICoreFlowTask toTask)
        {
            var transitionsFromTask = GetTaskTransitions(flow, fromTask);

            var validTransitions = new List<ICoreFlowTransition>();

            foreach (var currentTransition in transitionsFromTask)
            {
                if (currentTransition.ToTask == toTask)
                {
                    validTransitions.Add(currentTransition);
                    continue;
                }

                var currentTaskValidTransitions = GetPathsFromTaskToTask(flow, currentTransition.ToTask, toTask);
                if (!currentTaskValidTransitions.Any())
                    continue;

                validTransitions.Add(currentTransition);
                validTransitions.AddRange(currentTaskValidTransitions);
            }

            return validTransitions;
        }

        public bool IsFlowValid(ICoreFlowEntity flow)
        {
            if (flow.Tasks.Count(task => task.TaskType == CoreFlowTaskType.Start) != 1)
                throw new Exception("One start task is required.");

            if (!flow.Tasks.Any(task => task.TaskType == CoreFlowTaskType.End))
                throw new Exception("At least one end task is required.");

            if(!flow.Transitions.Any(trs => trs.FromTask.TaskType == CoreFlowTaskType.Start))
                throw new Exception("At least one transition must begin from the start task.");

            var endTasks = flow.Tasks.Where(task => task.TaskType == CoreFlowTaskType.End).ToList();
            foreach (var endTask in endTasks) 
            {
                if (!flow.Transitions.Any(trs => trs.ToTask == endTask))
                    throw new Exception("At least one transition must reach an end task.");
            }

            if (!flow.Transitions.Any(trs => trs.FromTask.TaskType == CoreFlowTaskType.Start))
                throw new Exception("At least one transition must begin from the start task.");

            var tasksWithTransitions = flow.Transitions.Where(c => c.FromTask.TaskType == CoreFlowTaskType.Action).Select(c => c.FromTask).Distinct().ToList();
            if(tasksWithTransitions.Count != flow.Tasks.Count(task => task.TaskType == CoreFlowTaskType.Action))
                throw new Exception("Action tasks must connect with each other .");

            return true;
        }
    }
}
