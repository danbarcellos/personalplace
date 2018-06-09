using System;

namespace Pactor.Infra.DAL.ORM
{
    public class InterceptorResult
    {
        public InterceptorResult(bool proceed = true, string failMessage = null, Exception exception = null)
        {
            Proceed = proceed;
            FailMessage = failMessage;
            Exception = exception;
        }

        public bool Proceed { get; }

        public string FailMessage { get; }

        public Exception Exception { get; }
    }
}