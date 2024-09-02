namespace URLTester.Application.ViewModels;

public class ResultViewModel<T> : ResultViewModel
{
    public T? Value { get; private set; }
	public double ElapsedSeconds { get; private set; }

	private void AddValue(T value)
    {
        Value = value;
    }
	private void AddValue(double elapsedMilliseconds)
	{
		ElapsedSeconds = elapsedMilliseconds;
	}
	public static ResultViewModel<T> OK(T value, string message, double elapsedMilliseconds)
    {
        var result = new ResultViewModel<T>();
        result.AddValue(value);
        result.Succeed(message);
        result.AddValue(elapsedMilliseconds);
        return result;
    }
}

public class ResultViewModel
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public Dictionary<string, string[]> ValidationErrors { get; private set; } = [];


    public static ResultViewModel OK(string message)
    {
        var result = new ResultViewModel();
        result.Succeed(message);
        return result;
    }

    public void BadRequest(string message, Dictionary<string, string[]> errors)
    {
        ValidationErrors = errors;
        Failed(message);
    }

    public void NotFound(string message)
    {
        Failed(message);
    }

    public void Conflict(string message)
    {
        Failed(message);
    }

    public void InternalServerError(string message)
    {
        Failed(message);
    }

    protected void Succeed(string message)
    {
        SetMessage(message);
        IsSuccess = true;
    }

    private void Failed(string message)
    {
        SetMessage(message);
        IsSuccess = false;
    }

    private void SetMessage(string message)
    {
        Message = message;
    }
}
