var MakeClass = function () {
    return function (args) {
        if (this instanceof arguments.callee) {
            if (typeof this.__construct == "function") this.__construct.apply(this, args);
        } else return new arguments.callee(arguments);
    };
}
var NewClass = function (variables, constructor, functions) {
    var retn = MakeClass();
    for (var key in variables) {
        retn.prototype[key] = variables[key];
    }
    for (var key in functions) {
        retn.prototype[key] = functions[key];
    }
    retn.prototype.__construct = constructor;
    return retn;
}
var canReadMembersCallBack = function (data) { }
var mycallback = function (data) { }
var DataManipulationRight = NewClass({
}, function (serviceUrl) {
    this.serviceUrl = serviceUrl;
}, {
    "IsGranted": function (objectType, memberName, oid, operation, callbackHandlers) {
        var objectHandle = objectType + "(" + oid + ")";
        var _data = "objectType='" + objectType + "'&memberName='" + memberName + "'&objectHandle='" + objectHandle + "'&operation='" + operation + "'";
        var callbackHandlerWrapper = function (data) {
            for (var key in callbackHandlers) {
                callbackHandlers[key](data["IsGranted"]);
            }
        }
        this.ajaxRequest(_data, "IsGranted", callbackHandlerWrapper, "mycallback");
    },
    "IsUserAllowed": function (callbackHandler) {
        var callbackHandlerWrapper = function (data) {
            callbackHandler(data["IsUserAllowed"]);
        }
        this.ajaxRequest(null, "IsUserAllowed", callbackHandlerWrapper, "mycallback");
    },
    "CanReadMembers": function (objectType, membersName, oids, callbackHandler) {
        for (var key in oids) {
            oids[key] = objectType + "(" + oids[key] + ")";
        }
        var _data = "objectType='" + objectType + "'&membersName='" + membersName.join(";") + "'&targetObjectsHandle='" + oids.join(";") + "'";
        var callbackHandlerWrapper = function (data) {
            var canReadMembers = new Array();
            for (var key in data) {
                var keyValue = data[key].split(";");
                canReadMembers[keyValue[0]] = keyValue[1];
            }
            callbackHandler(canReadMembers);
        }
        this.ajaxRequest(_data, "CanReadMembers", callbackHandlerWrapper, "canReadMembersCallBack");
    },
    "ajaxRequest": function (_data, serviceOperationName, callbackHandler, callbackMethodName) {
        var _url = this.serviceUrl + "/" + serviceOperationName + "?$format=json&$callback=" + callbackMethodName + this.userCredentials();
        $.ajax({
            type: "GET",
            url: _url,
            data: _data,
            async: true,
            timeout: 10000,
            contentType: "application/json; charset=utf-8",
            jsonpCallback: callbackMethodName,
            dataType: "jsonp",
            success: function (data) {
                if (callbackHandler) {
                    callbackHandler(data.d);
                }
            },
            error: function (xhr, status, error) {
                if (callbackHandler) {
                    callbackHandler(error);
                }
            }
        });
    },
    "userCredentials": function () {
        return "&UserName=" + DXTremeClient.currentUser.UserName() + "&Password=" + DXTremeClient.currentUser.Password() + "";
    }
});