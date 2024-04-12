namespace Wolverine.Weather.Domain.Exceptions
{
    public class InsertWeatherForecastValidationException : Exception
    {
        public InsertWeatherForecastValidationException()
        { }

        public InsertWeatherForecastValidationException(string message) : base(message)
        { }

        public InsertWeatherForecastValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
