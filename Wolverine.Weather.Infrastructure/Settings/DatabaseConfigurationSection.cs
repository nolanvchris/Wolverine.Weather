namespace Wolverine.Weather.Infrastructure.Settings
{
    public class DatabaseConfigurationSection
    {
        public int CommandTimeout { get; set; } = 30;
        public int RetryAttempts { get; set; } = 3;
        public ConnectionStringsSection? ConnectionStrings { get; set; } //This object used to be declared before the nested class.
        public class ConnectionStringsSection
        {
            public string? WolverineDB { get; set; }
        }
    }
}
