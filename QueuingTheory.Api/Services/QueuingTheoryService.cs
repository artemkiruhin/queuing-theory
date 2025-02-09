using QueuingTheory.Api.Models;

namespace QueuingTheory.Api.Services;

public class QueuingTheoryService
{
    public QueueingResults CalculateMM1(double lambda, double mu)
    {
        if (lambda >= mu)
            throw new ArgumentException("System is unstable: arrival rate must be less than service rate");

        var rho = lambda / mu;
            
        return new QueueingResults
        {
            Utilization = rho,
            L = rho / (1 - rho),
            Lq = Math.Pow(rho, 2) / (1 - rho),
            W = 1 / (mu - lambda),
            Wq = rho / (mu - lambda),
            P0 = 1 - rho
        };
    }

    public QueueingResults CalculateMMC(double lambda, double mu, int c)
    {
        if (c < 1)
            throw new ArgumentException("Number of servers must be positive");

        var rho = lambda / (mu * c);
        if (rho >= 1)
            throw new ArgumentException("System is unstable: arrival rate must be less than total service rate");

        // Calculate P0 (probability of empty system)
        double sum = 0;
        for (var n = 0; n < c; n++)
            sum += Math.Pow(c * rho, n) / Factorial(n);
            
        var p0 = 1 / (sum + (Math.Pow(c * rho, c) / (Factorial(c) * (1 - rho))));

        // Calculate Lq (average queue length)
        var lq = (p0 * Math.Pow(lambda / mu, c) * rho) / 
                 (Factorial(c) * Math.Pow(1 - rho, 2));

        return new QueueingResults
        {
            Utilization = rho,
            Lq = lq,
            L = lq + lambda / mu,
            Wq = lq / lambda,
            W = (lq / lambda) + (1 / mu),
            P0 = p0
        };
    }

    private double Factorial(int n)
    {
        if (n == 0) return 1;
        double result = 1;
        for (var i = 1; i <= n; i++)
            result *= i;
        return result;
    }
}