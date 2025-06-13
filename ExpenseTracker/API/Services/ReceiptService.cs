using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repository;
        private readonly IExpenseRepository _expenseRepo;
        private readonly IWebHostEnvironment _env;

        public ReceiptService(IReceiptRepository repository, IExpenseRepository expenseRepo, IWebHostEnvironment env)
        {
            _repository = repository;
            _expenseRepo = expenseRepo;
            _env = env;
        }

        public async Task UploadReceiptAsync(Guid userId, Guid expenseId, IFormFile file)
        {
            var expense = await _expenseRepo.GetByIdAsync(expenseId, userId);
            if (expense == null || expense.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized or invalid expense");

            var folderPath = Path.Combine(_env.ContentRootPath, "Receipts");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var receipt = new Receipt
            {
                ExpenseId = expenseId,
                FileName = file.FileName,
                FilePath = filePath,
                ContentType = file.ContentType
            };

            await _repository.AddAsync(receipt);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReceiptDto>> GetReceiptsForExpenseAsync(Guid userId, Guid expenseId)
        {
            var expense = await _expenseRepo.GetByIdAsync(expenseId, userId);
            if (expense == null || expense.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized or invalid expense");

            var receipts = await _repository.GetByExpenseIdAsync(expenseId);
            return receipts.Select(r => new ReceiptDto
            {
                Id = r.Id,
                FileName = r.FileName,
                FilePath = r.FilePath,
                ContentType = r.ContentType,
                UploadedAt = r.UploadedAt
            });
        }

        public async Task<(byte[] content, string contentType, string fileName)> DownloadReceiptAsync(Guid userId, Guid receiptId)
        {
            var receipt = await _repository.GetByIdAsync(receiptId);
            if (receipt == null || receipt.Expense.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized or invalid receipt");

            var fileBytes = await File.ReadAllBytesAsync(receipt.FilePath!);
            return (fileBytes, receipt.ContentType ?? "application/octet-stream", receipt.FileName);
        }
        

        public async Task DeleteReceiptAsync(Guid userId, Guid receiptId)
        {
            var receipt = await _repository.GetByIdAsync(receiptId);
            if (receipt == null || receipt.Expense.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized or invalid receipt");
                
            if (!string.IsNullOrEmpty(receipt.FilePath) && File.Exists(receipt.FilePath))
            {
                File.Delete(receipt.FilePath);
            }

            await _repository.DeleteAsync(receipt);
            await _repository.SaveChangesAsync();
        }

    }
}