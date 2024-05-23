﻿using System.Net;

namespace CashFlow.Exception.ExceptionBase;
public class ErrorOnValidationException : CashFlowException
{
    private readonly List<string> _errors;

    public override int statusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return _errors;
    }
}
