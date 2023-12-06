using dotnetCancellationToken.Models;

namespace dotnetCancellationToken.Service
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(CancellationToken cancellationToken);
        Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
        Task DeleteTransactionAsync(int id, CancellationToken cancellationToken);

    }
}
