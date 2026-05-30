namespace Akaru.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class AcessoNegadoException : DomainException
{
    public AcessoNegadoException(string message) : base(message)
    {
    }
}
