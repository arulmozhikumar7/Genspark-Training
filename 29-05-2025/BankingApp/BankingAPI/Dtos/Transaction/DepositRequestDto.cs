namespace BankingAPI.DTOs
{
    public class DepositRequestDto
{
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

}