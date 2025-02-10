namespace QueuingTheory.Api.Models;

public class QueueingResults
{
    public double Utilization { get; set; }          // ρ (rho)
    public double L { get; set; }                    // Average number in system
    public double Lq { get; set; }                   // Average number in queue
    public double W { get; set; }                    // Average time in system
    public double Wq { get; set; }                   // Average time in queue
    public double P0 { get; set; }                   // Probability of empty system
}