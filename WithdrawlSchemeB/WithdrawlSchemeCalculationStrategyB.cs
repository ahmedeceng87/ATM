using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Interfaces;

namespace WithdrawlSchemeB
{
    public class WithdrawlSchemeCalculationStrategyB : ICalculationStrategy
    {
        #region Fields

        private readonly int[] _denominationCounter = new int[12];

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
            var availaibleDenominations = DenominationManagement.GetAvailableDenominations();
            return availaibleDenominations.Aggregate<KeyValuePair<decimal, int>, decimal>(0, (current, item) => current + (item.Key * item.Value));
        }

        #endregion

        #region Private Methods

        private void ResetCounters()
        {
            Array.Clear(_denominationCounter, 0, _denominationCounter.Length);
        }

        private bool IsSufficientCashAvailable(decimal amount)
        {
            var availaibleDenominations = DenominationManagement.GetAvailableDenominations();
            var availaibleDenominationsCountClone = new Dictionary<decimal, int>(availaibleDenominations);
            var isSufficientCashAvailable =
                IsRequiredDenominationAvailable(amount, availaibleDenominationsCountClone, _denominationCounter);
            if (isSufficientCashAvailable)
            {
                DenominationManagement.SetAvailableDenominations(availaibleDenominationsCountClone);
            }
            return isSufficientCashAvailable;
        }

        private static bool IsRequiredDenominationAvailable(decimal amount,
                                                           IDictionary<decimal, int> availableDenominations,
                                                           IList<int> counter)
        {
            var preferredDenomination = DenominationManagement.GetPreferredDenomination();
            // first process preferred denomination
            if (availableDenominations.ContainsKey(preferredDenomination))
            {
                var index = availableDenominations.Keys.ToList().IndexOf(preferredDenomination);
                var denomination = availableDenominations.ElementAt(index);
                amount = GetAmountBasedOnAvailableDenominations(amount, availableDenominations, counter, denomination, index);
            }

            for (int index = 0; index < availableDenominations.Count; index++)
            {
                // exit from loop if amount is 0
                if (amount == 0)
                {
                    break;
                }

                var denomination = availableDenominations.ElementAt(index);

                // skip preferred denomination as it is already processed earlier
                if (denomination.Key == preferredDenomination)
                {
                    continue;
                }
                amount = GetAmountBasedOnAvailableDenominations(amount, availableDenominations, counter, denomination, index);
            }

            return amount == 0;
        }

        private static decimal GetAmountBasedOnAvailableDenominations(decimal amount, 
                                                                      IDictionary<decimal, int> availableDenominations,
                                                                      IList<int> counter, 
                                                                      KeyValuePair<decimal, int> denomination, 
                                                                      int index)
        {
            if (amount >= denomination.Key)
            {
                var requiredDenominationCount = (int) (amount/denomination.Key);
                if (denomination.Value >= requiredDenominationCount)
                {
                    amount = amount - requiredDenominationCount*denomination.Key;
                    counter[index] = requiredDenominationCount;
                    availableDenominations[denomination.Key] = denomination.Value - counter[index];
                }
                else
                {
                    amount = amount - (denomination.Value*denomination.Key);
                    counter[index] = denomination.Value;
                    availableDenominations[denomination.Key] = 0;
                }
            }
            return amount;
        }

        private string PrintCurrency()
        {
            var availaibleDenominations = DenominationManagement.GetAvailableDenominations();
            var poundsFormattedString = PrintPassedInDenominations(availaibleDenominations, _denominationCounter);
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
                    var currencySymbol = index < DenominationManagement.PenceStartIndex ? "£" : "p";
                    var formattedDenomination = index < DenominationManagement.PenceStartIndex ? denomination.Key : denomination.Key * 100;
                    formattedString = string.Format("{0}\n{1} {2} X {3}.", formattedString, currencySymbol, formattedDenomination, counter[index]);
                }
            }
            return formattedString;
        }

        #endregion
    }
}
