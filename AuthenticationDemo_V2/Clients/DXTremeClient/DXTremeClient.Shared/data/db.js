
(function () {
    DXTremeClient.currentUser = {
        Password: ko.observable(""),
        UserName: ko.observable("")
    };
    var endpointSelector = new DevExpress.EndpointSelector(DXTremeClient.config.endpoints);

    var serviceConfig = $.extend(true, {}, DXTremeClient.config.services, {
        db: {
            url: endpointSelector.urlFor("db"),
            jsonp: !window.WinJS,

            errorHandler: handleServiceError,
            beforeSend: function (sender) {
                sender.params.UserName = DXTremeClient.currentUser.UserName();
                sender.params.Password = DXTremeClient.currentUser.Password();
            }
        }
    });

    function handleServiceError(error) {
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
                if (window.WinJS) {
                    try {
                        new Windows.UI.Popups.MessageDialog(error.message).showAsync();
                    } catch (e) {
                        // Another dialog is shown
                    }
                } else {
                    alert(error.message);
                }
            }
        }
    }

    // Enable partial CORS support for IE < 10
    if($.browser.msie)
        $.support.cors = true;
    
    DXTremeClient.db = new DevExpress.data.ODataContext(serviceConfig.db);

    DXTremeClient.db.addErrorHandler = function (handler) {
        errorHandler = handler;
    };

}());
