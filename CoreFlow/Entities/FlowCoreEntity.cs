namespace CoreFlow.Model.Entities
{
    using CoreFlow.Model.Interfaces;
    using System.Collections.Generic;

    public class FlowCoreEntity : FlowCoreBaseEntity, IFlowCoreEntity
    {
        public IList<IFlowCoreTask> Tasks { get; set; }
        public IList<IFlowCoreTransition> Transitions { get; set; }

    }
}
