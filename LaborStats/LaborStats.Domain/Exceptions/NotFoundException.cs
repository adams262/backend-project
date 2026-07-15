
using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Domain.Exceptions;

namespace LaborStats.Domain.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key \"{key}\" was not found.") { }
}
