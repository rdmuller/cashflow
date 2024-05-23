﻿namespace CashFlow.Exception.ExceptionBase;
public abstract class CashFlowException : SystemException
{
    protected CashFlowException(string message) : base(message)
    {

    }

    public abstract int statusCode { get; }

    public abstract List<string> GetErrors();
}
