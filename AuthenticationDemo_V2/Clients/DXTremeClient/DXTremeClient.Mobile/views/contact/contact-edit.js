
DXTremeClient.ContactEdit = function(params) {
    var viewModel = {
        ErrorMessage: ko.observable(),
        contact: new DXTremeClient.ContactViewModel(),
        dbValidationErrorHandle: function (error) {
            if (error.httpStatus == 403) {
                viewModel.ErrorMessage("Security error!\r\n" + error.description);
                var overlay = $("#overlay").data("dxOverlay");
                overlay.show();
            }
        },

        handleSave: function() {
            if(params.id !== undefined)
                viewModel.update();
            else
                viewModel.insert();
        },

        load: function() {
            var self = this;
            DXTremeClient.db.Contact.byKey(params.id).done(function(data) {
                self.contact.fromJS(data);
            });
        },
        update: function(){
            DXTremeClient.db.Contact.update(params.id, viewModel.contact.toJS_Save()).done(function () {
                DXTremeClient.app.navigate("ContactDetails/" + params.id);
            });
        },

        insert: function() {
            DXTremeClient.db.Contact.insert(viewModel.contact.toJS()).done(function () {
                DXTremeClient.app.navigate("Contact");
            });
        },

        viewShown: function () {
            var self = this;
            DXTremeClient.db.addErrorHandler(self.dbValidationErrorHandle);
            if (params.id !== undefined) {
                this.load();
            }
        },

        hideOverlay: function () {
            var overlay = $("#overlay").data("dxOverlay");
            overlay.hide();
        }
    };
    return viewModel;
};
