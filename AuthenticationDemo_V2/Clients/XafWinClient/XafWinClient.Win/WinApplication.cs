using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

namespace XafWinClient.Win {
    public partial class XafWinClientWindowsFormsApplication : WinApplication {
        public XafWinClientWindowsFormsApplication() {
            InitializeComponent();
            DelayedViewItemsInitialization = true;
        }
        protected override void CheckCompatibilityCore() {
            //base.CheckCompatibilityCore();
        }
        private void XafWinClientWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            throw new InvalidOperationException(
                "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                "or manually create a database using the 'DBUpdater' tool.\r\n" +
                "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " +
                "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
        }
    }
}
