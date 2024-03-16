using System.Data;

namespace Wolverine.Weather.Domain.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        int CommandTimeout { get; }
        int RetryAttempts { get; }
        IDbConnection GetWeatherDbConnection(); //Question: Does it matter what the encapsulation type is in an interface? (public, private, protectected ect...)
    }
}
