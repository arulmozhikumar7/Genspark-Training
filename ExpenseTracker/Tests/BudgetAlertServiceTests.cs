using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Hubs;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Dtos;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Tests.Services
{
    public class BudgetAlertServiceTests
    {
        private readonly Mock<IHubContext<BudgetHub>> _hubContextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly BudgetAlertService _service;

        public BudgetAlertServiceTests()
        {
            _hubContextMock = new Mock<IHubContext<BudgetHub>>();
            _clientProxyMock = new Mock<IClientProxy>();

            var clientsMock = new Mock<IHubClients>();
            clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);
            _hubContextMock.Setup(h => h.Clients).Returns(clientsMock.Object);

            _service = new BudgetAlertService(_hubContextMock.Object);
        }

        [Theory]
        [InlineData(100, 0, "Budget limit exceeded!")]
        [InlineData(100, 5, "90% of budget used.")]
        [InlineData(100, 49, "50% of budget used.")]
        [InlineData(100, 60, null)]
        public async Task CheckAndNotifyAsync_SendsCorrectAlert(decimal limitAmount, decimal balanceAmount, string expectedMessage)
        {
            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                Name = "Test Budget",
                LimitAmount = limitAmount,
                BalanceAmount = balanceAmount,
                UserId = Guid.NewGuid(),
                IsDeleted = false,
                Category = new Category { Name = "Groceries" }
            };

            await _service.CheckAndNotifyAsync(budget);

            if (expectedMessage == null)
            {
                _clientProxyMock.Verify(p =>
                    p.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default),
                    Times.Never);
            }
            else
            {
                _clientProxyMock.Verify(p =>
                    p.SendCoreAsync(
                        "BudgetAlert",
                        It.Is<object[]>(args => IsExpectedAlert(args, expectedMessage)),
                        default),
                    Times.Once);
            }
        }

        private bool IsExpectedAlert(object[] args, string expectedMessage)
        {
            if (args.Length == 0)
                return false;

            if (args[0] is not BudgetAlertDto dto)
                return false;

            return dto.Message == expectedMessage;
        }
    }
}
