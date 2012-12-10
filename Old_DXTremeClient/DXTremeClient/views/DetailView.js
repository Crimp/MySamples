DXTremeClient.DetailView = function (params) {
    var model = {
        FirstName: ko.observable(),
        LastName: ko.observable(),
        Email: ko.observable(),
        Photo: ko.observable("noimage"),
        CanEdit: CanEdit = ko.observable(false),
        ObjectType: ko.observable(),
        UILevelSecurity: UILevelSecurity = ko.observable(true),
        handleBackClick: function (e) {
            DXTremeClient.app.navigate("_back");
        },
        canEdit: function (callbackHandler) {
            DXTremeClient.DataManipulationRight.IsGranted(model.ObjectType(), "", params.oid, "Write", callbackHandler);
        },
        handleEditClick: function (e) {
            var uri = DXTremeClient.app.router.format({
                action: "ContactEditView",
                oid: params.oid,
                index: params.oid
            });
            DXTremeClient.app.navigate(uri);
        },
        viewShown: function () {
            DXTremeClient.db.Contact.load({
                filter: [DXTremeClient.db.Contact._key, new DevExpress.data.Guid(params.oid)]
            }).done(createDetailContent);
        }
    };
    var createDetailContent = function (list) {
        model.ObjectType(list[0].__metadata.type);

        var mambers = new Array("LastName", "Email");
        var oids = new Array(params.oid);
        DXTremeClient.DataManipulationRight.CanReadMembers(model.ObjectType(), mambers, oids, loadData);

        model.FirstName(list[0].FirstName);
        model.LastName(list[0].LastName);
        model.Email(list[0].Email);
        model.Photo(DXTremeClient.getImageUrl(list[0].Photo));
        model.canEdit(CanEdit);
    };
    var loadData = function(data){
        var objectHandle =  model.ObjectType() + "(" + params.oid + ")";
        if (data[(objectHandle + "LastName")] === "False") {
            model.LastName("Protected Content");
        }
        if (data[(objectHandle + "Email")] === "False") {
            model.Email("Protected Content");
        }
    }
    return model;
}