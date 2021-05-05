using System;
namespace CoreFlow.Model.Exceptions
{
    public class CoreFlowException: Exception
    {
        public CoreFlowException()
        {
        }

        public CoreFlowException(string message)
        : base(message)
        {
        }

        public CoreFlowException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
