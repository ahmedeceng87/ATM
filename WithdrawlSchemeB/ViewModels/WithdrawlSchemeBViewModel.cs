using System;
using System.Collections.ObjectModel;
using Interfaces.Interfaces;
using Interfaces.ModularEvents;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System.Linq;

namespace WithdrawlSchemeB.ViewModels
{
    public class WithdrawlSchemeBViewModel : NotificationObject
    {
        #region Fields

        private readonly ICalculationStrategy _calculationStrategy;
        private readonly IEventAggregator _eventAggregator;
        private string _denominationInfo = string.Empty;
        private decimal _preferredDenomination;
        private readonly ObservableCollection<Tuple<decimal, string>> _formattedDenominations = new ObservableCollection<Tuple<decimal, string>>();

        #endregion

        #region Properties

        public string DenominationInfo
        {
            get
            {
                return _denominationInfo;
            }
            set
            {
                _denominationInfo = value;
                RaisePropertyChanged(() => DenominationInfo);
            }
        }

        public decimal PreferredDenomination
        {
            get
            {
                return _preferredDenomination;
            }
            set
            {
                _preferredDenomination = value;
                DenominationManagement.SetPreferredDenomination(value);
                RaisePropertyChanged(() => PreferredDenomination);
            }
        }

        public decimal BalanceLeft
        {
            get
            {
                return _calculationStrategy.GetBalanceLeft();
            }
        }

        public ObservableCollection<Tuple<decimal, string>> FormattedDenominations
        {
            get
            {
                return _formattedDenominations;
            }
        }

        #endregion

        #region Constructor

        public WithdrawlSchemeBViewModel(ICalculationStrategy calculationStrategy,
                                         IEventAggregator eventAggregator)
        {
            _calculationStrategy = calculationStrategy;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EnterWithdrawlAmountEvent>().Subscribe(EnterWithdrawlAmountEventHandler);
            _preferredDenomination = DenominationManagement.GetPreferredDenomination();
            RefreshAvailableDenominations();
        }

        #endregion

        #region Private Methods

        private void EnterWithdrawlAmountEventHandler(decimal withdrawlAmount)
        {
            var info = _calculationStrategy.GetDenominationInfo(withdrawlAmount).Trim();
            var preferredDenomTuple = _formattedDenominations.FirstOrDefault(itm => itm.Item1 == _preferredDenomination);

            var preferredDenomString = string.Empty;
            if (preferredDenomTuple != null)
            {
                preferredDenomString = string.Format("Preferred denomination for this withdrawl was {0}", preferredDenomTuple.Item2);
            }

            DenominationInfo = string.Format("{0}\n{1}", info, preferredDenomString);
            RaisePropertyChanged(() => BalanceLeft);
            RefreshAvailableDenominations();
        }

        private void RefreshAvailableDenominations()
        {
            _formattedDenominations.Clear();
            var availableDenominations = DenominationManagement.GetAvailableDenominations();
            for (int index = 0; index < availableDenominations.Count; index++)
            {
                var denomination = availableDenominations.ElementAt(index);
                if (denomination.Value > 0)
                {
                    var currencySymbol = index < DenominationManagement.PenceStartIndex ? "£" : "p";
                    var formattedDenomination = index < DenominationManagement.PenceStartIndex ? denomination.Key : denomination.Key * 100;
                    _formattedDenominations.Add(Tuple.Create(denomination.Key, string.Format("{0} {1}", currencySymbol, formattedDenomination)));
                }
            }

            var preferredDenom = _formattedDenominations.FirstOrDefault(itm => itm.Item1 == _preferredDenomination);
            if (preferredDenom == null
                &&
                _formattedDenominations.Any())
            {
                _preferredDenomination = _formattedDenominations.FirstOrDefault().Item1;
            }

            RaisePropertyChanged(() => FormattedDenominations);
            RaisePropertyChanged(() => PreferredDenomination);
        }

        #endregion
    }
}
