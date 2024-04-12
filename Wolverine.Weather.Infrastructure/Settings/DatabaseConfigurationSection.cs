﻿namespace Wolverine.Weather.Infrastructure.Settings
{
    public class DatabaseConfigurationSection
    {
        public int CommandTimeout { get; set; } = 30;
        public int RetryAttempts { get; set; } = 3;
        public ConnectionStringsSection? ConnectionStrings { get; set; } //This object used to be declared before the nested class.
        public StoredProcedureNames StoredProcedureNames { get; set; }
    }

    /// <summary>
    /// Just a definition for a subsection of database configuration.
    /// </summary>
    public class ConnectionStringsSection
    {
        public string? WolverineDB { get; set; }
    }

    /// <summary>
    /// Another definition of a subsection of database configuration.
    /// </summary>
    public class StoredProcedureNames
    {
        public string WeatherForecastInsertStoredProcedureName { get; set; }
        public string WeatherForecastGetByIdStoredProcedureName { get; set; }
        public string WeatherForecastGetAllStoredProcedureName { get; set; }
    }
}
