using BankingAPI.DTOs;
using BankingAPI.Models;

namespace BankingAPI.Mappers
{
    public static class BankAccountMapper
    {
        public static BankAccount MapCreateDtoToBankAccount(BankAccountCreateDto dto)
        {
            return new BankAccount
            {
              
                UserId = dto.UserId
            };
        }

        public static BankAccountReadDto MapBankAccountToReadDto(BankAccount bankAccount)
        {
            return new BankAccountReadDto
            {
                Id = bankAccount.Id,
                AccountNumber = bankAccount.AccountNumber
            };
        }
    }
}
