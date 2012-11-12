using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using ODataDemoServiceBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ClientServer_Wcf_StandartAuth.Server {
    public class Global : System.Web.HttpApplication {
        public static string ConnectionString;
        protected void Application_Start(object sender, EventArgs e) {
            ConnectionString = ConfigurationManager.ConnectionStrings["AzureApplicationServices"].ConnectionString;

            CustomAuthenticationServiceHelper customAuthenticationServiceHelper = new CustomAuthenticationServiceHelper();
            ValueManager.ValueManagerType = typeof(ASPRequestValueManager<>).GetGenericTypeDefinition();
            QueryRequestSecurityStrategyHandler securityProviderHandler = delegate() {
                return new SecurityStrategyComplex(typeof(SecuritySystemUser), typeof(SecuritySystemRole), new AuthenticationStandard());
            };
            SecuredDataServer dataServer = new SecuredDataServer(
                    ConnectionString, customAuthenticationServiceHelper.XPDictionary, securityProviderHandler);
            CustomWcfSecuredDataServer.SetSecuredDataServer(dataServer);
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {

        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}