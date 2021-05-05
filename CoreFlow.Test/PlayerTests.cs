namespace CoreFlow.Test
{
    using NUnit.Framework;
    using CoreFlow.Engine;
    using CoreFlow.Model.Entities;
    using CoreFlow.Model.Enums;

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

            // Tasks
            var taskStart = new CoreFlowTask("Start", CoreFlowTaskType.Start);
            var taskAction = new CoreFlowTask("Action", CoreFlowTaskType.Action);
            var taskEnd = new CoreFlowTask("End", CoreFlowTaskType.End);

            testFlow.Tasks.Add(taskStart);
            testFlow.Tasks.Add(taskAction);
            testFlow.Tasks.Add(taskEnd);

            // Transitions
            var transitionStartToAction = new CoreFlowTransition("Start", taskStart, taskAction);
            var transitionActionToEnd = new CoreFlowTransition("Complete", taskAction, taskEnd);

            testFlow.Transitions.Add(transitionStartToAction);
            testFlow.Transitions.Add(transitionActionToEnd);

            // Actions
            Assert.IsTrue(_engine.StartFlow(testFlow));

            var activeTasks = _engine.GetActiveTasks(testFlow);

            foreach (var activeTask in activeTasks)
            {
                Assert.IsTrue(_engine.CompleteTask(testFlow, activeTask));
            }

            Assert.AreEqual(CoreFlowStatus.Completed, testFlow.Status);
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

            testFlow.Tasks.Add(taskStart);
            testFlow.Tasks.Add(taskAction);
            testFlow.Tasks.Add(taskEnd1);

            // Transitions
            var transitionStartToAction = new CoreFlowTransition("Start", taskStart, taskAction);
            var transitionActionToEnd1 = new CoreFlowTransition("Complete", taskAction, taskEnd1);
            var transitionActionToEnd2 = new CoreFlowTransition("Fail", taskAction, taskEnd2);

            testFlow.Transitions.Add(transitionStartToAction);
            testFlow.Transitions.Add(transitionActionToEnd1);
            testFlow.Transitions.Add(transitionActionToEnd2);

            FixIds(testFlow);

            // Actions
            Assert.IsTrue(_engine.StartFlow(testFlow));

            Assert.AreEqual(CoreFlowTaskStatus.Active, taskAction.Status);

            Assert.IsTrue(_engine.CompleteTask(testFlow, taskAction, transitionActionToEnd1.Id));

            Assert.AreEqual(CoreFlowStatus.Completed, testFlow.Status);
            Assert.AreEqual(CoreFlowTaskStatus.Completed, taskEnd1.Status);
            Assert.AreEqual(CoreFlowTaskStatus.NotStarted, taskEnd2.Status);
        }
    }
}
