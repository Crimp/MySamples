using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDemoServiceBase {
    public class ASPRequestValueManager<ValueType> : IValueManager<ValueType> {
        private string requestEntryName;
        public ASPRequestValueManager() {
            requestEntryName = Guid.NewGuid().ToString();
        }
        public ValueType Value {
            get {
                if(CanManageValue)
                    return (ValueType)HttpContext.Current.Items[requestEntryName];
                else
                    return default(ValueType);
            }
            set {
                HttpContext.Current.Items[requestEntryName] = value;
            }
        }
        public bool CanManageValue {
            get {
                if(HttpContext.Current == null)
                    return false;
                if(HttpContext.Current.Request == null)
                    return false;
                return true;
            }
        }
        public void Clear() {
            Value = default(ValueType);
        }
        private HttpRequest Request {
            get {
                if(HttpContext.Current == null) {
                    throw new InvalidOperationException("HttpContext.Current is null");
                }
                if(HttpContext.Current.Request == null)
                    throw new InvalidOperationException("HttpContext.Current.Request is null");
                return HttpContext.Current.Request;
            }
        }
    }
}