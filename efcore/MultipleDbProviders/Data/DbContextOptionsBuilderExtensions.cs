using Microsoft.Extensions.Configuration;

namespace Data;

public static class DbContextOptionsBuilderExtensions
{
	const string Mssql = "mssql";
	const string Sqlite = "sqlite";
	const string Npgsql = "npgsql";

	/// <summary>
	/// Specify database to be used
	/// </summary>
	/// <param name="provider">database provider</param>
	/// <returns>The DbContextOptionsBuilder with configured database</returns>
	/// <exception cref="InvalidOperationException"></exception>
	/// <exception cref="NullReferenceException"></exception>
	public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, IConfiguration config, string? provider)
	{
		if (string.IsNullOrWhiteSpace(provider))
			throw new NullReferenceException("No selected provider");

		provider = provider.ToLowerInvariant().Trim();

		return provider switch
		{
			Mssql => builder.UseSqlServer(config.GetConnectionString("MSSQLConnection"), options => options.MigrationsAssembly("Migrators.Mssql")),

			Sqlite => builder.UseSqlite(config.GetConnectionString("DefaultConnection"), options => options.MigrationsAssembly("Migrators.Sqlite")),

			Npgsql => builder.UseNpgsql(config.GetConnectionString("NPGSQLConnection"), options => options.MigrationsAssembly("Migrators.Npgsql")),

			_ => throw new InvalidOperationException($"Unsupported provider: {provider}")
		};
	}
}

