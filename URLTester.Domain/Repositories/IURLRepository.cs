using URLTester.Domain.Models;

namespace URLTester.Domain.Repositories;

public interface IURLRepository
{
    public Task AddAsync(URL url, CancellationToken cancellationToken);
    public Task UpdateAsync(URL url, CancellationToken cancellationToken);
    public Task<URL?> GetByOriginalAsync(string original, CancellationToken cancellationToken);
    public Task<URL?> GetByShortenedAsync(string shortened, CancellationToken cancellationToken);
	public Task<URL?> GetByURLIdAsync(int UrlId, CancellationToken cancellationToken);

	public Task<IReadOnlyList<URL>> GetAllAsync(CancellationToken cancellationToken);
}
