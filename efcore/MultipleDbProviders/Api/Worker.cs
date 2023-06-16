using Microsoft.EntityFrameworkCore;

namespace Api;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IDbContextFactory<SampleDbContext> _dbContextFactory;

    public Worker(IDbContextFactory<SampleDbContext> dbContextFactory, ILogger<Worker> logger)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (await dbContext.Database.CanConnectAsync(cancellationToken))
        {
            using var tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogDebug("Started an explicit database transaction... {transId}", tx.TransactionId);

            if (!dbContext.People.Any())
            {
                _logger.LogDebug("Seeding database...");
                await dbContext.People.AddRangeAsync(Enumerable.Range(0, 3).Select(_ => new Person
                {
                    Firstname = Faker.Name.First(),
                    Lastname = Faker.Name.Last()
                }), cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await tx.CommitAsync(cancellationToken);
            _logger.LogDebug("Committed database transaction... {transId}", tx.TransactionId);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

