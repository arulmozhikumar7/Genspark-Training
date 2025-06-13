using ExpenseTrackerAPI.DTOs;
using Microsoft.AspNetCore.Http;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IReceiptService
    {
        Task UploadReceiptAsync(Guid userId, Guid expenseId, IFormFile file);
        Task<IEnumerable<ReceiptDto>> GetReceiptsForExpenseAsync(Guid userId, Guid expenseId);
        Task<(byte[] content, string contentType, string fileName)> DownloadReceiptAsync(Guid userId, Guid receiptId);
        Task DeleteReceiptAsync(Guid userId, Guid receiptId);

    }
}