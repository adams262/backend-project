
using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Exceptions;

public class ImportAlreadyExistsException : AppException
{
    public ImportAlreadyExistsException(string message) : base(message) { }
}
