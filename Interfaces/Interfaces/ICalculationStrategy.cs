
namespace Interfaces.Interfaces
{
    public interface ICalculationStrategy
    {
        string GetDenominationInfo(decimal withdrawlAmount);

        decimal GetBalanceLeft();
    }
}
