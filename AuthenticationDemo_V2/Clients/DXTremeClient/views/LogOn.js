DXTremeClient.LogOn = function (params) {
    var viewModel = {
        Password: ko.observable(""),
        UserName: ko.observable(""),
        handleLogOnClick: function (e) {
            if (viewModel.UserName()) {
                DXTremeClient.currentUser.UserName(viewModel.UserName());
                DXTremeClient.currentUser.Password(viewModel.Password())
                DXTremeClient.DataManipulationRight.IsUserAllowed(isUserAllowed);
            }
        },
        hideOverlay: function () {
            var overlay = $("#logonFailedoverlay").data("dxOverlay");
            overlay.hide();
        },
    };
    var isUserAllowed = function (userAllowed) {
        if (userAllowed) {
            DXTremeClient.app.navigate("");
        } else {
            var overlay = $("#logonFailedoverlay").data("dxOverlay");
            overlay.show();
        }
    };
    return viewModel;
};