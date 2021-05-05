namespace CoreFlow.Test
{
    using CoreFlow.Model.Entities;

    public abstract class TestBase
    {
        public void FixIds(CoreFlowEntity flow)
        {
            for (int i = 0; i < flow.Tasks.Count; i++)
            {
                flow.Tasks[i].Id = i;
            }

            for (int i = 0; i < flow.Transitions.Count; i++)
            {
                flow.Transitions[i].Id = i;
            }
        }
    }
}
