namespace ROP;
public static class ResultExtensions
{
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        string error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value) ? result : Result<T>.Failure(error);
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> mappingFunc)
    {
        return result.IsSuccess ?
            mappingFunc(result.Value) :
            Result<TOut>.Failure(result.Error);
    }
}