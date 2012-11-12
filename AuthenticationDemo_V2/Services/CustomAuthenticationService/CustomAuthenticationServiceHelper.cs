using BusinessObjectsLibrary;
using DataProvider;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;

namespace CustomAuthenticationService {
    public class CustomAuthenticationServiceHelper : DataServiceHelper {
        public CustomAuthenticationServiceHelper() :
            this(Global.ConnectionString, new Assembly[] { typeof(Contact).Assembly }, "BusinessObjectsLibrary") {
        }
        public CustomAuthenticationServiceHelper(string connectionString, Assembly[] assemblies, string namespaceName) :
            base(connectionString, assemblies, namespaceName) {
        }
        protected override ISelectDataSecurity GetSelectDataSecurity() {
            return ((SecurityStrategy)SecuritySystem.Instance).CreateSelectDataSecurity();
        }
    }
}