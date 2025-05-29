
using BankingAPI.DTOs;
using BankingAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _service;

        public TransactionController(TransactionService service)
        {
            _service = service;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionRequestDto dto)
        {
            var result = await _service.DepositAsync(dto);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDto dto)
        {
            var result = await _service.WithdrawAsync(dto);
            return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionRequestDto dto)
        {
            var result = await _service.TransferAsync(dto);
            return Ok(result);
        }
    }
}
