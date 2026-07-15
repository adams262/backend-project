
using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
    protected AppException(string message, Exception innerException) : base(message, innerException) { }
}
