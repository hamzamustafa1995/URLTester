using Microsoft.EntityFrameworkCore;
using URLTester.Domain.Models;
using URLTester.Domain.Repositories;

namespace URLTester.Infrastructure.Implementations;

public class URLRepository(URLTesterDBContext dbContext) : IURLRepository
{
    public async Task AddAsync(URL url, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(url, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(URL url, CancellationToken cancellationToken)
	{  // Attach the entity to the context if it's not already being tracked
		dbContext.Entry(url).State = EntityState.Modified;

		await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<URL?> GetByOriginalAsync(string original, CancellationToken cancellationToken)
    {
        var url = await dbContext.Set<URL>().FirstOrDefaultAsync(x => x.Original == original, cancellationToken);
        return url;
    }

    public async Task<URL?> GetByShortenedAsync(string shortened, CancellationToken cancellationToken)
    {
        var url = await dbContext.Set<URL>().FirstOrDefaultAsync(x => x.Shortened.Contains(shortened), cancellationToken);
        return url;
    }

    public async Task<IReadOnlyList<URL>> GetAllAsync(CancellationToken cancellationToken)
    {
        var urls = await dbContext.Set<URL>().ToListAsync(cancellationToken);
        //range operator [..]
        urls = [.. urls.OrderByDescending(x => x.Id)];
        return urls;

    }

	public async Task<URL?> GetByURLIdAsync(int UrlId, CancellationToken cancellationToken)
	{
		var url = await dbContext.Set<URL>().FirstOrDefaultAsync(x => x.Id == UrlId, cancellationToken);
		return url;
	}
}
