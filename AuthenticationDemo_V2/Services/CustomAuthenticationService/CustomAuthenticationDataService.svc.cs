using BusinessObjectsLibrary;
using DataProvider;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace CustomAuthenticationService {
    [JSONPSupportBehaviorAttribute]
    public class CustomAuthenticationDataService : DataServiceBase {
        static CustomAuthenticationDataService() {
            AddServiceOperation("IsGranted");
            AddServiceOperation("IsUserAllowed");
            AddServiceOperation("CanReadMembers");
        }
        public CustomAuthenticationDataService()
            : this(new HttpContextWrapper(HttpContext.Current)) {
        }
        public CustomAuthenticationDataService(HttpContextBase httpContext)
            : this(httpContext, new CustomAuthenticationServiceHelper(), "CustomAuthenticationService") {
        }
        public CustomAuthenticationDataService(HttpContextBase httpContext, DataServiceHelper dataServiceHelper, string containerName)
            : base(httpContext, dataServiceHelper, containerName) {
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
