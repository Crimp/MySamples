using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

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
