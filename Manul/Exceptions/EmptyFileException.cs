using System;

namespace Manul.Exceptions;

[Serializable]
public class EmptyFileException : Exception
{
    public EmptyFileException() {}
    public EmptyFileException(string message) : base(message) {}
    public EmptyFileException(string message, Exception inner) : base(message, inner) {}
}