using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using System.ServiceModel.Activation;

namespace ClientServer_Wcf_StandartAuth.Server {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomWcfSecuredDataServer : WcfSecuredDataServer {
        private static SecuredDataServer securedDataServer;
        public static void SetSecuredDataServer(SecuredDataServer dataServer) {
            securedDataServer = dataServer;
        }
        public CustomWcfSecuredDataServer() : base(securedDataServer) { }

    }
}
