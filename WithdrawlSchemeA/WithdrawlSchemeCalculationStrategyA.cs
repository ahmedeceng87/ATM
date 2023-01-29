using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Interfaces;

namespace WithdrawlSchemeA
{
    public class WithdrawlSchemeCalculationStrategyA : ICalculationStrategy
    {
        #region Fields

        private const int PenceStartIndex = 6;
        private readonly int[] _denominationCounter = new int[12];

        // dictionary with key= denomination and value = total count available (of that denomination)
        // can be moved to some generic place
        private Dictionary<decimal, int> _availaibleDenominations = new Dictionary<decimal, int>
        {
            { 50, 50 },
            { 20, 50 },
            { 10, 50 },
            { 5, 50 },
            { 2, 100 },
            { 1, 100 },

            { (decimal) 0.50, 100 },
            { (decimal) 0.20, 100 },
            { (decimal) 0.10, 100 },
            { (decimal) 0.05, 100 },
            { (decimal) 0.02, 100 },
            { (decimal) 0.01, 100 }
        };

        #endregion

        #region Interface Implementation

        public string GetDenominationInfo(decimal withdrawlAmount)
        {
            var denominationInfo = IsSufficientCashAvailable(withdrawlAmount) ?
                                        PrintCurrency() : "Cash or required denomination is not sufficient.";
            ResetCounters();
            return denominationInfo;
        }

        public decimal GetBalanceLeft()
        {
            return _availaibleDenominations.Aggregate<KeyValuePair<decimal, int>, decimal>(0, (current, item) => current + (item.Key * item.Value));
        }

        #endregion

        #region Private Methods

        private void ResetCounters()
        {
            Array.Clear(_denominationCounter, 0, _denominationCounter.Length);
        }

        private bool IsSufficientCashAvailable(decimal amount)
        {
            var availaibleDenominationsCountClone = new Dictionary<decimal, int>(_availaibleDenominations);
            var isSufficientCashAvailable =
                IsRequiredDenominationAvailable(amount, availaibleDenominationsCountClone, _denominationCounter);
            if (isSufficientCashAvailable)
            {
                _availaibleDenominations = availaibleDenominationsCountClone;
            }
            return isSufficientCashAvailable;
        }

        private static bool IsRequiredDenominationAvailable(decimal amount,
                                                           IDictionary<decimal, int> availableDenominations,
                                                           IList<int> counter)
        {
            for (int index = 0; index < availableDenominations.Count; index++)
            {
                var denomination = availableDenominations.ElementAt(index);
                if (amount >= denomination.Key)
                {
                    var requiredDenominationCount = (int)(amount / denomination.Key);
                    if (denomination.Value >= requiredDenominationCount)
                    {
                        amount = amount - requiredDenominationCount * denomination.Key;
                        counter[index] = requiredDenominationCount;
                        availableDenominations[denomination.Key] = denomination.Value - counter[index];
                    }
                    else
                    {
                        amount = amount - (denomination.Value * denomination.Key);
                        counter[index] = denomination.Value;
                        availableDenominations[denomination.Key] = 0;
                    }

                    // exit from loop if amount is 0
                    if (amount == 0)
                    {
                        break;
                    }
                }
            }

            return amount == 0;
        }

        private string PrintCurrency()
        {
            var poundsFormattedString = PrintPassedInDenominations(_availaibleDenominations, _denominationCounter);
            return string.Format("{0}", poundsFormattedString);
        }

        private string PrintPassedInDenominations(Dictionary<decimal, int> availableDenominations,
                                                  IList<int> counter)
        {
            var formattedString = string.Empty;
            for (var index = 0; index < counter.Count; index++)
            {
                var denomination = availableDenominations.ElementAt(index);
                if (counter[index] != 0)
                {
                    var currencySymbol = index < PenceStartIndex ? "£" : "p";
                    var formattedDenomination = index < PenceStartIndex ? denomination.Key : denomination.Key * 100;
                    formattedString = string.Format("{0}\n{1} {2} X {3}.", formattedString, currencySymbol, formattedDenomination, counter[index]);
                }
            }
            return formattedString;
        }

        #endregion
    }
}
