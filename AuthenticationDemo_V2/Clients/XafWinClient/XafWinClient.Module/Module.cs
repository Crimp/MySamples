using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security.Strategy;
using BusinessObjectsLibrary;


namespace XafWinClient.Module {
    public sealed partial class XafWinClientModule : ModuleBase {
        public XafWinClientModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        protected override IEnumerable<Type> GetDeclaredExportedTypes() {
            List<Type> result = new List<Type>(base.GetDeclaredExportedTypes());
            result.AddRange(new Type[] { typeof(SecuritySystemUser), typeof(SecuritySystemRole), typeof(Contact) });
            return result;
        }
    }
}
