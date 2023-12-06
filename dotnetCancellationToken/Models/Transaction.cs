namespace dotnetCancellationToken.Models;

public class Transaction
{
    public int Id { get; set; }
    public Ticker Ticker { get; set; }
    public decimal Amount { get; set; }
    public long Quantity { get; set; }
    public DateTime Date { get; set; }
}

public class Ticker(string code, string companyName)
{
    public string Code { get; } = code;
    public string CompanyName { get; } = companyName;

    public override string ToString()
    {
        return $"{Code} - {CompanyName}";
    }
}
