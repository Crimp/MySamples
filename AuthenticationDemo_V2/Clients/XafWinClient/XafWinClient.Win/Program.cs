using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Remoting;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using System.ServiceModel;

namespace XafWinClient.Win {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            XafWinClientWindowsFormsApplication winApplication = new XafWinClientWindowsFormsApplication();

            SetupForWCFServer(winApplication);
        }
        private static void SetupForWCFServer(WinApplication winApplication) {
            string connectionString = "http://samplexafsecurity.cloudapp.net/CustomWcfSecuredDataServer.svc";
            try {
                EndpointAddress endpointAddress = new EndpointAddress(connectionString);
                BasicHttpBinding bh = new BasicHttpBinding();
                System.ServiceModel.Channels.Binding b = WcfDataServerHelper.CreateDefaultBinding();
                WcfSecuredDataServerClient clientDataServer = new WcfSecuredDataServerClient(
                    b, endpointAddress);

                ServerSecurityClient securityClient = new ServerSecurityClient(clientDataServer, new ClientInfoFactory());
                winApplication.ApplicationName = "ClientServer_CustomAuth";
                winApplication.Security = securityClient;
                winApplication.CreateCustomObjectSpaceProvider += delegate(object sender, CreateCustomObjectSpaceProviderEventArgs e) {
                    e.ObjectSpaceProvider = new DataServerObjectSpaceProvider(clientDataServer, securityClient);
                };
                winApplication.CreateCustomLogonWindowObjectSpace += delegate(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e) {
                    e.ObjectSpace = ((XafApplication)sender).CreateObjectSpace();
                };

                winApplication.Setup();
                winApplication.Start();

                clientDataServer.Close();
            } catch(Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
