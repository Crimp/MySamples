
DXTremeClient.ContactDetails = function (params) {
    var hidePopup = function () {
        var popup = $("#deleteConfirmation").data("dxPopup");
        popup.hide();
    };

    var viewModel = {
        SecurityDataLoaded: ko.observable(false),
        ErrorMessage: ko.observable(),
        CanEdit: ko.observable(true),
        ObjectType: ko.observable(),
        UILevelSecurity: ko.observable(true),

        id: params.id,

        contact: new DXTremeClient.ContactViewModel(),
        handleEditClick: function (e) {
            DXTremeClient.app.navigate("ContactEdit/" + params.id);
        },
        securityDataLoaded: function () {
            viewModel.SecurityDataLoaded(true);
        },
        canEdit: function (callbackHandler) {
            var self = this;
            var callbackHandlers = new Array(callbackHandler, viewModel.securityDataLoaded);
            DXTremeClient.DataManipulationRight.IsGranted(self.ObjectType(), "", params.id, "Write", callbackHandlers);
        },
        dbValidationErrorHandle: function (error) {
            if (error.httpStatus == 403) {
                viewModel.ErrorMessage("Security error!\r\n" + error.description);
                var overlay = $("#overlay").data("dxOverlay");
                overlay.show();
            }
        },

        hideOverlay: function () {
            var overlay = $("#overlay").data("dxOverlay");
            overlay.hide();
        },

        handleDelete: function () {
            var overlay = $("#deleteConfirmation").data("dxPopup");
            overlay.show();
        },

        handleConfirmDelete: function () {
            DXTremeClient.db.Contact.remove(params.id).done(function () {
                DXTremeClient.app.navigate("Contact", { target: "blank" });
            }).fail(hidePopup);
        },

        handleCancelDelete: function () {
            hidePopup();
        },
        viewShown: function () {
            contactDetails = this;
            DXTremeClient.db.addErrorHandler(contactDetails.dbValidationErrorHandle);
            DXTremeClient.db.Contact.byKey(params.id).done(function (data) {
                contactDetails.ObjectType(data.__metadata.type);
                contactDetails.canEdit(contactDetails.CanEdit);
                contactDetails.contact.fromJS(data);
            });
        }
    };
    return viewModel;
};
