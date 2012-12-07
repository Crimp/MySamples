
DXTremeClient.Contact = function (params) {
    var dataSource = ko.observableArray();
    return {
        ErrorMessage: ko.observable(),
        dataSource: dataSource,
        dataSource: {
            store: DXTremeClient.db.Contact,
            sort: [{ field: "FirstName", desc: false }],
            select: ["oid", "FirstName", "LastName", "Photo"],
            map: function (item) {
                return new DXTremeClient.ContactViewModel(item);
            }
        },
        handleLogOffClick: function (e) {
            DXTremeClient.currentUser.UserName("");
            DXTremeClient.currentUser.Password("");
            DXTremeClient.app.navigate("LogOn/null");
        }
    };
};
