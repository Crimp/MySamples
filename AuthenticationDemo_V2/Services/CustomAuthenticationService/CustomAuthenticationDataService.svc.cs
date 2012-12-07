using BusinessObjectsLibrary;
using DataProvider;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using DevExpress.Xpo.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;

namespace CustomAuthenticationService {
    [JSONPSupportBehaviorAttribute]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CustomAuthenticationDataService : XpoDataServiceV3, System.Data.Services.IRequestHandler {
        private DataServiceHelper _dataServiceHelper;
        public CustomAuthenticationDataService()
            : this(new HttpContextWrapper(HttpContext.Current)) {
        }
        public CustomAuthenticationDataService(HttpContextBase httpContext)
            : this(httpContext, new CustomAuthenticationServiceHelper(), "CustomAuthenticationService") {
        }
        public CustomAuthenticationDataService(HttpContextBase httpContext, DataServiceHelper dataServiceHelper, string containerName) :
            base(new MyContext(containerName, dataServiceHelper.NamespaceName, dataServiceHelper.CreateDataLayer())) {
            if((httpContext == null) && (HttpContext.Current == null)) {
                throw new ArgumentNullException("context", "The context cannot be null if not running on a Web context.");
            }
            _dataServiceHelper = dataServiceHelper;
        }
        public static void InitializeService(DataServiceConfiguration config) {
            //config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetEntitySetAccessRule("Contact", EntitySetRights.All);
            config.SetServiceOperationAccessRule("IsGranted", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("IsUserAllowed", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("CanReadMembers", ServiceOperationRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.DataServiceBehavior.AcceptProjectionRequests = true;
        }
        protected override void HandleException(HandleExceptionArgs args) {
            if(args.Exception.GetType() == typeof(ObjectLayerSecurityException)) {
                //Forbidden
                args.Exception = new DataServiceException(403, args.Exception.Message);
            }
            base.HandleException(args);
        }

        [WebGet]
        public bool IsGranted(string objectType, string memberName, string objectHandle, string operation) {
            Type type = XafTypesInfo.Instance.FindTypeInfo(objectType).Type;
            return ((IRequestSecurity)SecuritySystem.Instance).IsGranted(new ClientPermissionRequest(type, memberName, objectHandle, operation));
        }
        [WebGet]
        public IEnumerable<string> CanReadMembers(string objectType, string membersName, string targetObjectsHandle) {
            List<string> _membersName = new List<string>(membersName.Split(';'));
            List<string> _targetObjectsHandle = new List<string>(targetObjectsHandle.Split(';'));
            Type type = XafTypesInfo.Instance.FindTypeInfo(objectType).Type;
            Dictionary<string, bool> canReadMembers = ((IRequestSecurity)SecuritySystem.Instance).CanReadMembers(type.AssemblyQualifiedName, _membersName, _targetObjectsHandle);
            List<string> result = new List<string>();
            foreach(KeyValuePair<string, bool> item in canReadMembers) {
                result.Add(item.Key + ";" + item.Value);
            }
            return result;
        }
        [WebGet]
        public bool IsUserAllowed() {
            return true;
        }
    }
}
