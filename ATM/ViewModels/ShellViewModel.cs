using System.Windows;
using Interfaces.ModularEvents;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;

namespace ATM.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        #region Fields

        private string _withdrawlAmount;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Relay Commands

        private DelegateCommand _withdrawlEnteredCmd;

        public DelegateCommand WithdrawlEnteredCmd
        {
            get
            {
                return _withdrawlEnteredCmd ?? (_withdrawlEnteredCmd = new DelegateCommand(OnWithdrawlEnteredCmdHandler));
            }
        }

        #endregion

        #region Constructor

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion

        #region Properties


        public string WithdrawlAmount
        {
            get
            {
                return _withdrawlAmount;
            }
            set
            {
                _withdrawlAmount = value;
                RaisePropertyChanged(() => WithdrawlAmount);
            }
        }

        #endregion

        #region Private Methods

        private void OnWithdrawlEnteredCmdHandler()
        {
            decimal amount;
            var validationString = ValidateWithdrawlAmount(_withdrawlAmount, out amount);

            if (!string.IsNullOrEmpty(validationString))
            {
                MessageBox.Show(validationString);
                return;
            }

            var enterWithdrawlAmountEvent = _eventAggregator.GetEvent<EnterWithdrawlAmountEvent>();
            enterWithdrawlAmountEvent.Publish(amount);
        }

        private string ValidateWithdrawlAmount(string withdrawlAmount, out decimal amount)
        {
            var errMsg = string.Empty;

            if (string.IsNullOrEmpty(_withdrawlAmount))
            {
                errMsg= "Withdrawl amount should not be empty";
                amount = -1; 
                return errMsg;
            }

            if (!decimal.TryParse(withdrawlAmount, out amount))
            {
                errMsg = "Invalid withdrawl amount";
            }

            else if (amount <= 0)
            {
                errMsg ="Withdrawl amount should be greater than 0";
            }

            return errMsg;
        }

        #endregion
    }
}
