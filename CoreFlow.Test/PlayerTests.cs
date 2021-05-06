namespace CoreFlow.Test
{
    using NUnit.Framework;
    using CoreFlow.Engine;
    using CoreFlow.Model.Entities;
    using CoreFlow.Model.Enums;
    using System;

    [TestFixture]
    public class PlayerTests : TestBase
    {
        private CoreFlowEngine _engine;

        [SetUp]
        public void SetUp()
        {
            _engine = new();
        }

        [Test]
        public void Engine_SimpleFlow()
        {
            CoreFlowEntity testFlow = new();

            bool onActivatedFlowWasRun = false;
            bool onCompletedFlowWasRun = false;

            testFlow.OnActivated = () =>
            {
                onActivatedFlowWasRun = true;
            };

            testFlow.OnCompleted = () =>
            {
                onCompletedFlowWasRun = true;
            };

            bool onActivatedTaskWasRun = false;
            bool onCompletedTaskWasRun = false;

            // Tasks
            var taskStart = new CoreFlowTask("Start", CoreFlowTaskType.Start);
            var taskAction = new CoreFlowTask("Action", CoreFlowTaskType.Action);
            taskAction.OnActivated = () => {
                onActivatedTaskWasRun = true;
                Console.WriteLine("Task Action was activated");
            };

            taskAction.OnCompleted = () => {
                onCompletedTaskWasRun = true;
                Console.WriteLine("Task Action was completed");
            };

            var taskEnd = new CoreFlowTask("End", CoreFlowTaskType.End);

            testFlow.AddTask(taskStart);
            testFlow.AddTask(taskAction);
            testFlow.AddTask(taskEnd);

            // Transitions
            var transitionStartToAction = new CoreFlowTransition("Start", taskStart, taskAction);
            var transitionActionToEnd = new CoreFlowTransition("Complete", taskAction, taskEnd);

            testFlow.AddTransition(transitionStartToAction);
            testFlow.AddTransition(transitionActionToEnd);

            // Actions
            Assert.IsTrue(_engine.StartFlow(testFlow));

            var activeTasks = _engine.GetActiveTasks(testFlow);

            foreach (var activeTask in activeTasks)
            {
                Assert.IsTrue(_engine.CompleteTask(testFlow, activeTask));
            }

            Assert.AreEqual(CoreFlowStatus.Completed, testFlow.Status);

            Assert.IsTrue(onActivatedTaskWasRun);
            Assert.IsTrue(onCompletedTaskWasRun);

            Assert.IsTrue(onActivatedFlowWasRun);
            Assert.IsTrue(onCompletedFlowWasRun);
        }

        [Test]
        public void Engine_ForkFlow()
        {
            CoreFlowEntity testFlow = new();

            // Tasks
            var taskStart = new CoreFlowTask("Start", CoreFlowTaskType.Start);
            var taskAction = new CoreFlowTask("Action", CoreFlowTaskType.Action);
            var taskEnd1 = new CoreFlowTask("End1", CoreFlowTaskType.End);
            var taskEnd2 = new CoreFlowTask("End2", CoreFlowTaskType.End);

            testFlow.AddTask(taskStart);
            testFlow.AddTask(taskAction);
            testFlow.AddTask(taskEnd1);

            // Transitions
            var transitionStartToAction = new CoreFlowTransition("Start", taskStart, taskAction);
            var transitionActionToEnd1 = new CoreFlowTransition("Complete", taskAction, taskEnd1);
            var transitionActionToEnd2 = new CoreFlowTransition("Fail", taskAction, taskEnd2);

            testFlow.AddTransition(transitionStartToAction);
            testFlow.AddTransition(transitionActionToEnd1);
            testFlow.AddTransition(transitionActionToEnd2);

            FixIds(testFlow);

            // Actions
            Assert.IsTrue(_engine.StartFlow(testFlow));

            Assert.AreEqual(CoreFlowTaskStatus.Active, taskAction.Status);

            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction, transitionActionToEnd1.Id));

            Assert.AreEqual(CoreFlowStatus.Completed, testFlow.Status);
            Assert.AreEqual(CoreFlowTaskStatus.Completed, taskEnd1.Status);
            Assert.AreEqual(CoreFlowTaskStatus.NotStarted, taskEnd2.Status);
        }

        [Test]
        public void Engine_ParallelFlow()
        {
            CoreFlowEntity testFlow = new();

            // Tasks
            var taskStart = new CoreFlowTask("Start", CoreFlowTaskType.Start);
            var taskAction1 = new CoreFlowTask("Action1", CoreFlowTaskType.Action);
            var taskAction2 = new CoreFlowTask("Action2", CoreFlowTaskType.Action);
            var taskAction3 = new CoreFlowTask("Action3", CoreFlowTaskType.Action);
            var taskAction4 = new CoreFlowTask("Action4", CoreFlowTaskType.Action);
            var taskAction5 = new CoreFlowTask("Action5", CoreFlowTaskType.Action);
            var taskEnd1 = new CoreFlowTask("End1", CoreFlowTaskType.End);
            var taskEnd2 = new CoreFlowTask("End2", CoreFlowTaskType.End);

            testFlow.AddTask(taskStart);
            testFlow.AddTask(taskAction1);
            testFlow.AddTask(taskAction2);
            testFlow.AddTask(taskAction3);
            testFlow.AddTask(taskAction4);
            testFlow.AddTask(taskAction5);
            testFlow.AddTask(taskEnd1);

            // Transitions
            var transitionStartToAction1 = new CoreFlowTransition("Start", taskStart, taskAction1);
            var transitionStartToAction3 = new CoreFlowTransition("Start", taskStart, taskAction3);

            var transitionAction1ToAction2 = new CoreFlowTransition("1to2", taskAction1, taskAction2);
            var transitionAction2ToAction5 = new CoreFlowTransition("2to3", taskAction2, taskAction5);

            var transitionAction3ToAction4 = new CoreFlowTransition("3to4", taskAction3, taskAction4);
            var transitionAction4ToAction5 = new CoreFlowTransition("4to5", taskAction4, taskAction5);

            var transitionActionToEnd1 = new CoreFlowTransition("Complete", taskAction5, taskEnd1);
            var transitionActionToEnd2 = new CoreFlowTransition("Fail", taskAction5, taskEnd2);

            testFlow.AddTransition(transitionStartToAction1);
            testFlow.AddTransition(transitionStartToAction3);
            testFlow.AddTransition(transitionAction1ToAction2);
            testFlow.AddTransition(transitionAction2ToAction5);
            testFlow.AddTransition(transitionAction3ToAction4);
            testFlow.AddTransition(transitionAction4ToAction5);
            testFlow.AddTransition(transitionActionToEnd1);
            testFlow.AddTransition(transitionActionToEnd2);

            FixIds(testFlow);

            // TODO add to another unit test
            //EngineHelper helper = new EngineHelper();
            //var paths = helper.GetPathsFromTaskToTask(testFlow, taskAction3, taskAction5);

            // Actions
            Assert.IsTrue(_engine.StartFlow(testFlow));

            Assert.AreEqual(CoreFlowTaskStatus.Active, taskAction1.Status);
            Assert.AreEqual(CoreFlowTaskStatus.Active, taskAction3.Status);

            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction1));
            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction2));

            Assert.AreEqual(CoreFlowTaskStatus.NotStarted, taskAction5.Status);

            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction3));
            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction4));

            Assert.AreEqual(CoreFlowTaskStatus.Active, taskAction5.Status);
            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction5));

            Assert.AreEqual(CoreFlowStatus.Completed, testFlow.Status);

            Assert.Zero(_engine.GetActiveTasks(testFlow).Count);
        }
    }
}
