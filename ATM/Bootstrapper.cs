using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace ATM
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
           // var shell = new Shell(new EventAggregator());
            var shell = (Window)Container.Resolve(typeof(Shell), "", new CompositeResolverOverride());
            Application.Current.MainWindow = shell;
            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var uri = new Uri(executingAssembly.CodeBase);
            var directory = Path.GetDirectoryName(uri.LocalPath);
            if ((!string.IsNullOrEmpty(directory))
                 &&
                 (Directory.Exists(directory)))
            {
                try
                {
                    Directory.SetCurrentDirectory(directory);
                }

                catch (Exception exception)
                {
                    Debug.Assert(false, exception.Message);
                }
            }

            var moduleCatalog = new ModuleCatalog();
            var modulesToBeLoaded = GetModulesToLoad(directory);
            if (string.IsNullOrEmpty(directory))
            {
                return moduleCatalog;
            }

            modulesToBeLoaded.ForEach(module =>
            {
                var moduleNameAttribute = module.Element("ModuleName");
                var moduleTypeattribute = module.Element("ModuleType");
                var refAtribute = module.Element("Ref");
                if ((moduleNameAttribute == null)
                    ||
                    (moduleTypeattribute == null)
                    ||
                    (refAtribute == null))
                {
                    return;
                }
                moduleCatalog.AddModule(new ModuleInfo
                {
                    InitializationMode = InitializationMode.WhenAvailable,
                    ModuleName = moduleNameAttribute.Value,
                    ModuleType = moduleTypeattribute.Value,
                    Ref = new Uri(Path.Combine(directory, refAtribute.Value)).ToString()
                });
            });
            return moduleCatalog;
        } 

        private static List<XElement> GetModulesToLoad(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            path = Path.Combine(path,
                         "Modules.xml");

            var xdoc = XDocument.Load(path);

            return (xdoc.Descendants("Module").Where(module =>
            {
                var loadAttribute = module.Element("Load");
                return loadAttribute != null && loadAttribute.Value.Equals("True");
            })).ToList();
        }
    }
}
