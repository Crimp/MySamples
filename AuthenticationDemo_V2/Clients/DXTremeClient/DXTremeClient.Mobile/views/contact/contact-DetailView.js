
DXTremeClient.ContactDetails = function (params) {
    var hidePopup = function () {
        var popup = $("#deleteConfirmation").data("dxPopup");
        popup.hide();
    };

    var viewModel = {
        SecurityDataLoaded: ko.observable(false),
        ErrorMessage: ko.observable(),
        CanEdit: ko.observable(true),
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
            DXTremeClient.DataManipulationRight.IsGranted(self.contact.ObjectType, "", params.id, "Write", callbackHandlers);
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
                contactDetails.contact.fromJS(data);
                contactDetails.canEdit(contactDetails.CanEdit);

                //CanReadMembers
                var mambers = new Array("LastName", "Email");
                var oids = new Array(contactDetails.contact.oid());
                DXTremeClient.DataManipulationRight.CanReadMembers(contactDetails.contact.ObjectType, mambers, oids, contactDetails.loadData);
            });
        },
        loadData: function (data) {
            var objectHandle = viewModel.contact.ObjectType + "(" + viewModel.contact.oid() + ")";
            if (data[(objectHandle + "LastName")] === "False") {
                viewModel.contact.LastName("Protected Content");
            }
            if (data[(objectHandle + "Email")] === "False") {
                viewModel.contact.Email("Protected Content");
            }
        },
    };
    return viewModel;
};
