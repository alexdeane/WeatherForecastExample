namespace WeatherForecastExample.ApplicationCore.Abstraction;

public class ServiceResult<T>
{
    public T? Result { get; init; }
    public string? UserSafeError { get; init; }

    public ServiceResult()
    {
    }

    public ServiceResult(T? result)
        => Result = result;
}