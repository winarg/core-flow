namespace CoreFlow.Model.Interfaces
{
    using System.Collections.Generic;

    public interface IFlowCoreEntity : IFlowCoreBaseEntity
    {
        IList<IFlowCoreTask> Tasks { get; set; }
        IList<IFlowCoreTransition> Transitions { get; set; }
    }
}
