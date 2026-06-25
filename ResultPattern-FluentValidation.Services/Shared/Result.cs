namespace ResultPattern_FluentValidation.Services.Shared;

public enum ResultType
{
    Success,
    BadRequest,
    NotFound,
    Failure,
    ValidationError,
    SystemError
}

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public List<string> Errors { get; private set; } = new();
    public ResultType Type { get; private set; }

    private Result() { }

    public static Result<T> Success(T data, string message = "Operation successful.") =>
        new() { IsSuccess = true, Data = data, Message = message, Type = ResultType.Success };

    public static Result<T> NotFound(string message = "No data found.") =>
        new() { IsSuccess = false, Message = message, Type = ResultType.NotFound };

    public static Result<T> BadRequest(string message) =>
        new() { IsSuccess = false, Message = message, Type = ResultType.BadRequest };

    public static Result<T> Failure(string message = "Operation failed.") =>
        new() { IsSuccess = false, Message = message, Type = ResultType.Failure };

    public static Result<T> ValidationError(List<string> errors, string message = "Validation Failed.") =>
        new() { IsSuccess = false, Message = message, Errors = errors, Type = ResultType.ValidationError };
}
