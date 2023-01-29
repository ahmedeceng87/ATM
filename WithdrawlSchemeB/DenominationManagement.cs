using System.Collections.Generic;

namespace WithdrawlSchemeB
{
    public class DenominationManagement
    {
        public const int PenceStartIndex = 6;
        private static decimal _preferredDenomination = 20;

        // dictionary with key= denomination and value = total count available (of that denomination)
        private static Dictionary<decimal, int> _availaibleDenominations = new Dictionary<decimal, int>
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

        public static Dictionary<decimal, int> GetAvailableDenominations()
        {
            return _availaibleDenominations;
        }

        public static void SetAvailableDenominations(Dictionary<decimal, int> denominations)
        {
            _availaibleDenominations = denominations;
        }

        public static decimal GetPreferredDenomination()
        {
            return _preferredDenomination;
        }

        public static void SetPreferredDenomination(decimal denomination)
        {
            _preferredDenomination = denomination;
        }
    }
}
