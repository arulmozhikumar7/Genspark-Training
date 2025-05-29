public class BankAccountReadDto
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
