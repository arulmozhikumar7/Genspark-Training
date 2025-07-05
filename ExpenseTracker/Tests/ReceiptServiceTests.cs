using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Services
{
    public class ReceiptServiceTests
    {
        private readonly Mock<IReceiptRepository> _receiptRepoMock;
        private readonly Mock<IExpenseRepository> _expenseRepoMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly ReceiptService _service;

        public ReceiptServiceTests()
        {
            _receiptRepoMock = new Mock<IReceiptRepository>();
            _expenseRepoMock = new Mock<IExpenseRepository>();
            _envMock = new Mock<IWebHostEnvironment>();
            _service = new ReceiptService(_receiptRepoMock.Object, _expenseRepoMock.Object, _envMock.Object);
        }

        [Fact]
        public async Task UploadReceiptAsync_UploadsFileAndSavesToDb()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expenseId = Guid.NewGuid();
            var expense = new Expense { Id = expenseId, UserId = userId };

            var fileMock = new Mock<IFormFile>();
            var content = "Fake content";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.ContentType).Returns("application/pdf");
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                    .Returns<Stream, System.Threading.CancellationToken>((stream, _) => ms.CopyToAsync(stream));

            _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _expenseRepoMock.Setup(r => r.GetByIdAsync(expenseId, userId)).ReturnsAsync(expense);
            _receiptRepoMock.Setup(r => r.AddAsync(It.IsAny<Receipt>())).Returns(Task.CompletedTask);
            _receiptRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            await _service.UploadReceiptAsync(userId, expenseId, fileMock.Object);

            // Assert
            _receiptRepoMock.Verify(r => r.AddAsync(It.IsAny<Receipt>()), Times.Once);
            _receiptRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetReceiptsForExpenseAsync_ReturnsReceiptDtos()
        {
            var userId = Guid.NewGuid();
            var expenseId = Guid.NewGuid();
            var expense = new Expense { Id = expenseId, UserId = userId };

            var receipts = new List<Receipt>
            {
                new Receipt { Id = Guid.NewGuid(), FileName = "file1.pdf", FilePath = "path1", ContentType = "application/pdf", UploadedAt = DateTime.UtcNow }
            };

            _expenseRepoMock.Setup(r => r.GetByIdAsync(expenseId, userId)).ReturnsAsync(expense);
            _receiptRepoMock.Setup(r => r.GetByExpenseIdAsync(expenseId)).ReturnsAsync(receipts);

            var result = await _service.GetReceiptsForExpenseAsync(userId, expenseId);

            Assert.Single(result);
            Assert.Equal("file1.pdf", result.First().FileName);
        }

        [Fact]
        public async Task DownloadReceiptAsync_ReturnsFileContent()
        {
            var userId = Guid.NewGuid();
            var expense = new Expense { UserId = userId };
            var receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                FilePath = Path.GetTempFileName(),
                FileName = "download.pdf",
                ContentType = "application/pdf",
                Expense = expense
            };

            await File.WriteAllTextAsync(receipt.FilePath, "Test content");

            _receiptRepoMock.Setup(r => r.GetByIdAsync(receipt.Id)).ReturnsAsync(receipt);

            var (content, contentType, fileName) = await _service.DownloadReceiptAsync(userId, receipt.Id);

            Assert.NotNull(content);
            Assert.Equal("application/pdf", contentType);
            Assert.Equal("download.pdf", fileName);
        }

        [Fact]
        public async Task DeleteReceiptAsync_DeletesReceiptAndFile()
        {
            var userId = Guid.NewGuid();
            var receiptId = Guid.NewGuid();
            var tempFile = Path.GetTempFileName();
            var expense = new Expense { UserId = userId };

            var receipt = new Receipt { Id = receiptId, FilePath = tempFile, Expense = expense };

            _receiptRepoMock.Setup(r => r.GetByIdAsync(receiptId)).ReturnsAsync(receipt);
            _receiptRepoMock.Setup(r => r.DeleteAsync(receipt)).Returns(Task.CompletedTask);
            _receiptRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            await _service.DeleteReceiptAsync(userId, receiptId);

            // Assert
            Assert.False(File.Exists(tempFile));
            _receiptRepoMock.Verify(r => r.DeleteAsync(receipt), Times.Once);
            _receiptRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
