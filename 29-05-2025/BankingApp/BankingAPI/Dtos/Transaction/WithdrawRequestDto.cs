namespace BankingAPI.DTOs
{
public class WithdrawRequestDto
{
    public int FromAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}


}