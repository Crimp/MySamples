
DXTremeClient.Contact = function (params) {
    var dataSource = ko.observableArray();
    return {
        ErrorMessage: ko.observable(),
        dataSource: dataSource,
        dataSource: {
            store: DXTremeClient.db.Contact,
            sort: [{ field: "FirstName", desc: false }],
            select: ["oid", "FirstName", "LastName", "Photo"],
            map: function(item) {
                return new DXTremeClient.ContactViewModel(item);
            }
        }
    };
};
