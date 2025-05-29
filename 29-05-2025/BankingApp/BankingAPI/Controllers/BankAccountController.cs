using Microsoft.AspNetCore.Mvc;
using BankingAPI.DTOs;
using BankingAPI.Services;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly BankAccountService _bankAccountService;

        public BankAccountController(BankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost]
        public async Task<ActionResult<BankAccountReadDto>> CreateBankAccount([FromBody] BankAccountCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdAccount = await _bankAccountService.CreateBankAccountAsync(createDto.UserId, createDto.AccountType);

            return CreatedAtAction(nameof(CreateBankAccount), new { id = createdAccount.Id }, createdAccount);
        }
        
         [HttpGet("balance/{accountNumber}")]
        public async Task<ActionResult<decimal>> GetAvailableBalance(string accountNumber)
        {
            try
            {
                var balance = await _bankAccountService.GetAvailableBalanceAsync(accountNumber);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
