using Interfaces.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WithdrawlSchemeB.Views;

namespace WithdrawlSchemeB
{
    public class WithdrawlSchemeModuleB : IModule
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        #endregion

        #region Constructor

        public  WithdrawlSchemeModuleB(IUnityContainer container,
                                       IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Interface Implementation

        public void Initialize()
        {
            _container.RegisterType<ICalculationStrategy, WithdrawlSchemeCalculationStrategyB>();
            _regionManager.RegisterViewWithRegion("DenominationRegion", typeof(WithdrawSchemeBView));
        }

        #endregion
    }
}
