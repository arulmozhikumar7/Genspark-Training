using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Tests.Controllers
{
    public class ReceiptControllerTests
    {
        private readonly Mock<IReceiptService> _mockService;
        private readonly ReceiptController _controller;
        private readonly Guid _userId;

        public ReceiptControllerTests()
        {
            _mockService = new Mock<IReceiptService>();
            _controller = new ReceiptController(_mockService.Object);

            _userId = Guid.NewGuid();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", _userId.ToString())
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Upload_ValidFile_ReturnsOk()
        {
            var expenseId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Fake content");
            writer.Flush();
            stream.Position = 0;
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.FileName).Returns("test.png");
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            await _controller.Upload(expenseId, fileMock.Object);

            _mockService.Verify(s => s.UploadReceiptAsync(_userId, expenseId, fileMock.Object), Times.Once);
        }

        [Fact]
        public async Task GetReceipts_ReturnsReceipts()
        {
            var expenseId = Guid.NewGuid();
            var receipts = new List<ReceiptDto>
            {
                new ReceiptDto { Id = Guid.NewGuid(), FileName = "test1.png" },
                new ReceiptDto { Id = Guid.NewGuid(), FileName = "test2.png" }
            };

            _mockService
                .Setup(s => s.GetReceiptsForExpenseAsync(_userId, expenseId))
                .ReturnsAsync(receipts.AsEnumerable()); 

            var result = await _controller.GetReceipts(expenseId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Download_ReturnsFile()
        {
            var receiptId = Guid.NewGuid();
            var content = new byte[] { 1, 2, 3 };
            var contentType = "image/png";
            var fileName = "test.png";

            _mockService
                .Setup(s => s.DownloadReceiptAsync(_userId, receiptId))
                .ReturnsAsync((content, contentType, fileName));

            var result = await _controller.Download(receiptId) as FileContentResult;

            Assert.NotNull(result);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal(content, result.FileContents);
            Assert.Equal(fileName, result.FileDownloadName);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            var receiptId = Guid.NewGuid();

            var result = await _controller.Delete(receiptId) as OkObjectResult;

            _mockService.Verify(s => s.DeleteReceiptAsync(_userId, receiptId), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
