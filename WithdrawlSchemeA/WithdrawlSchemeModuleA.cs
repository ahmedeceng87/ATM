using Interfaces.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WithdrawlSchemeA.Views;

namespace WithdrawlSchemeA
{
    public class WithdrawlSchemeModuleA : IModule
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        #endregion

        #region Constructor

        public WithdrawlSchemeModuleA(IUnityContainer container,
                                      IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Interface Implementation

        public void Initialize()
        {
            _container.RegisterType<ICalculationStrategy, WithdrawlSchemeCalculationStrategyA>();
            _regionManager.RegisterViewWithRegion("DenominationRegion", typeof(WithdrawSchemeAView));
        }

        #endregion
    }
}
