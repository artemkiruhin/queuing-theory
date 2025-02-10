namespace QueuingTheory.Api.Models;

public class QueueingModel
{
    public double ArrivalRate { get; set; }          // λ (lambda)
    public double ServiceRate { get; set; }          // μ (mu)
    public int Servers { get; set; } = 1;            // c (number of servers, 1 for M/M/1)
}