using Interfaces.Interfaces;
using Interfaces.ModularEvents;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace WithdrawlSchemeA.ViewModels
{
    public class WithdrawlSchemeAViewModel : NotificationObject
    {
        #region Fields

        private readonly ICalculationStrategy _calculationStrategy;
        private readonly IEventAggregator _eventAggregator;
        private string _denominationInfo;

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

        public decimal BalanceLeft
        {
            get
            {
                return _calculationStrategy.GetBalanceLeft();
            }
        }

        #endregion

        #region Constructor

        public WithdrawlSchemeAViewModel(ICalculationStrategy calculationStrategy,
                                         IEventAggregator eventAggregator)
        {
            _calculationStrategy = calculationStrategy;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<EnterWithdrawlAmountEvent>().Subscribe(EnterWithdrawlAmountEventHandler);
        }

        #endregion

        #region Private Methods

        private void EnterWithdrawlAmountEventHandler(decimal withdrawlAmount)
        {
            DenominationInfo = _calculationStrategy.GetDenominationInfo(withdrawlAmount).Trim();
            RaisePropertyChanged(()=> BalanceLeft);
        }

        #endregion
    }
}
