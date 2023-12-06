using dotnetCancellationToken.Models;
using dotnetCancellationToken.Service;
using Microsoft.AspNetCore.Mvc;

namespace dotnetCancellationToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionService transactionService) : ControllerBase
    {
        private readonly ITransactionService _transactionService = transactionService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(CancellationToken cancellationToken)
        {
            try
            {
                var timeoutCancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellation.Token);

                var transactions = await _transactionService.GetTransactionsAsync(linkedCts.Token);
                return Ok(transactions);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(500, "Operacao cancelada devido ao tempo limite.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar transacoes: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> AddTransaction(Transaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                var timeoutCancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellation.Token);

                var addedTransaction = await _transactionService.AddTransactionAsync(transaction, linkedCts.Token);
                return Ok(addedTransaction);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(500, "Opera��o cancelada.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar transa��o: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Transaction>> UpdateTransaction(int id, Transaction transaction, CancellationToken cancellationToken)
        {
            try
            {
                if (id != transaction.Id)
                {
                    return BadRequest("IDs n�o coincidem.");
                }

                var updatedTransaction = await _transactionService.UpdateTransactionAsync(transaction, cancellationToken);
                return Ok(updatedTransaction);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(500, "Opera��o cancelada.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar transa��o: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id, cancellationToken);
                return Ok("Transaction deleted");
            }
            catch (OperationCanceledException)
            {
                return StatusCode(500, "Opera��o cancelada.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir transa��o: {ex.Message}");
            }
        }
    }
}
