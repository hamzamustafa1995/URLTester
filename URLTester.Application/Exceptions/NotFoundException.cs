namespace URLTester.Application.Exceptions;

public class NotFoundException(string error) : Exception
{
    public string Error { get; } = error;
}