namespace ROP;
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string Error { get; private set; }

    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default!, error);
    }

    public bool IsFailure => !IsSuccess;

    public T GetValue()
    {
        if (IsSuccess)
        {
            return Value;
        }

        throw new InvalidOperationException("Cannot access value on a failed result.");
    }

    public string GetError()
    {
        if (IsFailure)
        {
            return Error;
        }

        throw new InvalidOperationException("Cannot access error on a successful result.");
    }

    public static Result<T> Create(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }
}