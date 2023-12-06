using Dapper;
using dotnetCancellationToken.Models;
using Npgsql;
using System.Data;

namespace dotnetCancellationToken.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        public TransactionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {

                await Task.Delay(10000, cancellationToken);

                await connection.OpenAsync(cancellationToken);


                const string query = "SELECT Id, TickerCode AS Ticker_Code, Amount, Quantity, Date FROM Transactions";
                return await connection.QueryAsync<Transaction>(query, cancellationToken);
            }
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                const string query = @"INSERT INTO Transactions (TickerCode, Amount, Quantity, Date) 
                                VALUES (@Code, @Amount, @Quantity, @Date)
                                RETURNING Id";

                var parameters = new
                {
                    Code = transaction.Ticker.Code,
                    Amount = transaction.Amount,
                    Quantity = transaction.Quantity,
                    Date = transaction.Date
                };

                await Task.Delay(5000, cancellationToken);


                var commandDefinition = new CommandDefinition(query, parameters, cancellationToken: cancellationToken);
                var result = await connection.QueryFirstOrDefaultAsync<int>(commandDefinition);              

                transaction.Id = result;

                return transaction;
            }
        }





        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                const string query = @"UPDATE Transactions 
                       SET Amount = @Amount, Quantity = @Quantity, Date = @Date
                       WHERE TickerCode = @Code
                       RETURNING Id";

                var commandDefinition = new CommandDefinition(query, transaction, cancellationToken: cancellationToken);
                await connection.QueryAsync(commandDefinition);

                return transaction;
            }
        }

        public async Task DeleteTransactionAsync(int id, CancellationToken cancellationToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                string query = $"DELETE FROM Transactions WHERE Id = {id}";
                var deletedTransaction = await connection.QueryFirstOrDefaultAsync<Transaction>(query, cancellationToken);
            }
        }
    }
}
