$(function () {
    DXTremeClient.currentUser = {
        Password: ko.observable(""),
        UserName: ko.observable("")
    };
    DXTremeClient.db = new DevExpress.data.EntityStoreContext({
        test: "",
        service: {
            url: DXTremeClient.serviceUrl,
            errorHandler: function (error) {
                if (error.httpStatus == 401) {
                    DXTremeClient.app.navigate("LogOn/null");
                    if (errorHandler) {
                        errorHandler(error);
                    }
                } else {
                    if (error.httpStatus == 403) {
                        if (errorHandler) {
                            errorHandler(error);
                        }
                    } else {
                        alert(error.message);
                    }
                }
            },
            beforeSend: function (sender) {
                sender.params.UserName = DXTremeClient.currentUser.UserName();
                sender.params.Password = DXTremeClient.currentUser.Password();
            },
        },
        entities: {
            Contact: { key: "oid" }
        }
    });
    DXTremeClient.db.addErrorHandler = function (handler) {
        errorHandler = handler;
    };
});