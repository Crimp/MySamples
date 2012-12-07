using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Xpo;

public class MyContext : XpoContext {
    public MyContext(string containerName, string namespaceName, IObjectLayer objectLayer)
        : base(containerName, namespaceName, objectLayer) { }
    public MyContext(string containerName, string namespaceName, IDataLayer dataLayer)
        : base(containerName, namespaceName, dataLayer) { }

    public override bool ShowLargePropertyAsNamedStream(Type classType, string propertyName) {
        //return base.ShowLargePropertyAsNamedStream(classType, propertyName);
        return false;
    }
    public override bool HideProperty(Type classType, string propertyName) {
        if(propertyName == "Oid") {
            return true;
        }
        return base.HideProperty(classType, propertyName);
    }
}