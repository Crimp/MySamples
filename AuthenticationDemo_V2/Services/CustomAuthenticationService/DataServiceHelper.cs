using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DataProvider {
    public class DataServiceHelper {
        private readonly object lockObject = new object();
        private static XPDictionary _xPDictionary = null;
        private static IObjectSpaceProvider objectSpaceProvider;
        static bool isInitialized = false;
        public DataServiceHelper(string connectionString, Assembly[] assemblies, string namespaceName) {
            this.ConnectionString = connectionString;
            this.Assemblies = assemblies;
            this.NamespaceName = namespaceName;

            Initialize(assemblies);
        }
        private void Initialize(Assembly[] assemblies) {
            lock(lockObject) {
                if(!isInitialized) {
                    isInitialized = true;
                    XpoTypesInfoHelper.ForceInitialize();
                    ITypesInfo info = XpoTypesInfoHelper.GetTypesInfo();
                    List<Assembly> _assemblies = new List<Assembly>(Assemblies);
                    _assemblies.Add(typeof(SecuritySystemUser).Assembly);
                    _assemblies.Add(typeof(DevExpress.ExpressApp.Xpo.Updating.ModuleInfo).Assembly);

                    foreach(Assembly ass in _assemblies) {
                        foreach(Type type in ModuleHelper.CollectExportedTypesFromAssembly(ass, IsExportedType)) {
                            info.RegisterEntity(type);
                        }
                    }

                    _xPDictionary = XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary;

                    SystemModule systemModule = new SystemModule();
                    systemModule.CustomizeTypesInfo(info);
                    ConnectionStringDataStoreProvider connectionDataStoreProvider = new ConnectionStringDataStoreProvider(ConnectionString);
                    objectSpaceProvider = new XPObjectSpaceProvider(connectionDataStoreProvider, info, XpoTypesInfoHelper.GetXpoTypeInfoSource());
                }
            }
        }

        public IObjectSpaceProvider ObjectSpaceProvider {
            get {
                return objectSpaceProvider;
            }
        }
        public XPDictionary XPDictionary {
            get {
                return _xPDictionary;
            }
        }
        NonPersistentExportedTypeHelper nonPersistentExportedTypeHelper;
        XpoExportedTypeHelper xpoExportedTypeHelper;
        public virtual Boolean IsExportedType(Type type) {
            if(nonPersistentExportedTypeHelper == null) {
                nonPersistentExportedTypeHelper = new NonPersistentExportedTypeHelper();
            }
            if(xpoExportedTypeHelper == null) {
                xpoExportedTypeHelper = new XpoExportedTypeHelper();
            }
            return nonPersistentExportedTypeHelper.IsExportedType(type) || xpoExportedTypeHelper.IsExportedType(type);
        }
        public virtual IObjectLayer CreateDataLayer() {
            SessionObjectLayer sessionObjectLayer = new SessionObjectLayer(CreateSession(AutoCreateOption.SchemaAlreadyExists), true, null, CreateSecurityRuleProvider(), null);
            return sessionObjectLayer;
        }
        public UnitOfWork CreateSession(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption) {
            IDataStore store = XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
            IDataLayer directDataLayer = new SimpleDataLayer(_xPDictionary, store);
            UnitOfWork directSession = new UnitOfWork(directDataLayer);
            return directSession;
        }

        public virtual Assembly[] Assemblies {
            get;
            set;
        }
        public string ConnectionString {
            get;
            set;
        }
        public string NamespaceName {
            get;
            set;
        }
        public virtual SecurityRuleProvider CreateSecurityRuleProvider() {
            ISelectDataSecurity selectDataSecurity = GetSelectDataSecurity();
            if(selectDataSecurity != null) {
                return new SecurityRuleProvider(_xPDictionary, selectDataSecurity);
            }
            return null;
        }
        protected virtual ISelectDataSecurity GetSelectDataSecurity() {
            return null;
        }
    }
}
