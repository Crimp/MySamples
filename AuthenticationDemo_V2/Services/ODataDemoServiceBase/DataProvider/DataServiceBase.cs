using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Web;
using DevExpress.Xpo;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.Security;
using System.Reflection;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo.Exceptions;
using System.ServiceModel.Web;

namespace DataProvider {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class DataServiceBase : XpoDataServiceV3, System.Data.Services.IRequestHandler {
        private static List<string> serviceOperations = new List<string>();
        private DataServiceHelper _dataServiceHelper;
        public DataServiceBase(HttpContextBase httpContext, DataServiceHelper dataServiceHelper, string containerName) :
            base(new MyContext(containerName, dataServiceHelper.NamespaceName, dataServiceHelper.CreateDataLayer())) {
            if((httpContext == null) && (HttpContext.Current == null)) {
                throw new ArgumentNullException("context", "The context cannot be null if not running on a Web context.");
            }
            _dataServiceHelper = dataServiceHelper;
        }
        public static void InitializeService(DataServiceConfiguration config) {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            foreach(string serviceOperationName in serviceOperations) {
                config.SetServiceOperationAccessRule(serviceOperationName, ServiceOperationRights.AllRead);
            }
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.DataServiceBehavior.AcceptProjectionRequests = true;
        }
        public DataServiceHelper DataServiceHelper {
            get {
                return _dataServiceHelper;
            }
        }
        protected override void HandleException(HandleExceptionArgs args) {
            if(args.Exception.GetType() == typeof(ObjectLayerSecurityException)) {
                //Forbidden
                args.Exception = new DataServiceException(403, args.Exception.Message);
            }
            base.HandleException(args);
        }

        public static void AddServiceOperation(string serviceOperationName) {
            serviceOperations.Add(serviceOperationName);
        }
    }
}
